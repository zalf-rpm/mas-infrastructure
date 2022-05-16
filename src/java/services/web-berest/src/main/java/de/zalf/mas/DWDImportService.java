package de.zalf.mas;

import clojure.java.api.Clojure;
import clojure.lang.IFn;
import org.capnproto.CallContext;
import org.capnproto.EzRpcServer;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.util.concurrent.CompletableFuture;
import java.util.zip.InflaterInputStream;
import java.util.Arrays;

public class DWDImportService {

    static class ImportService extends WebBerestDWDImport.DWLABImport.Server {

        //private final IFn currentDbFn;
        //private final IFn qFn;
        private final IFn importFn;
        //private final Object currentDb;

        public ImportService() {
            var requireFn = Clojure.var("clojure.core", "require");
            //requireFn.invoke(Clojure.read("de.zalf.berest.core.datomic"));
            //requireFn.invoke(Clojure.read("datomic.api"));

            //qFn = Clojure.var("datomic.api", "q");
            //currentDbFn = Clojure.var("de.zalf.berest.core.datomic", "current-db");
            //dbName = System.getProperty("berest.datomic.url");
            //currentDb = currentDbFn.invoke();//dbName);

            requireFn.invoke(Clojure.read("de.zalf.berest.core.import.dwd-data"));
            importFn = Clojure.var("de.zalf.berest.core.import.dwd-data", "import-dwd-data-into-datomic**");
        }

        @Override
        protected CompletableFuture<Void> importData(CallContext<WebBerestDWDImport.DWLABImport.ImportDataParams.Reader, WebBerestDWDImport.DWLABImport.ImportDataResults.Builder> context) {
            var params = context.getParams();
            var id = params.getId();
            //System.out.println("id: " + id);
            var dwla_comp = params.getDwla();
            var iis = new InflaterInputStream(new ByteArrayInputStream(dwla_comp.toArray()));
            StringBuffer dwla = new StringBuffer();
            byte[] buf = new byte[5];
            int rlen = -1;
            var aOk = false;
            try {
                while ((rlen = iis.read(buf)) != -1) {
                    dwla.append(new String(Arrays.copyOf(buf, rlen)));
                }
                //System.out.println("dwla:");
                aOk = (boolean) importFn.invoke("A", dwla.toString());
            } catch(IOException ioe) {}

            var dwlb_comp = params.getDwlb();
            iis = new InflaterInputStream(new ByteArrayInputStream(dwlb_comp.toArray()));
            StringBuffer dwlb = new StringBuffer();
            buf = new byte[5];
            rlen = -1;
            var bOk = false;
            try {
                while ((rlen = iis.read(buf)) != -1) {
                    dwlb.append(new String(Arrays.copyOf(buf, rlen)));
                    
                }
                //System.out.println("dwlb:");
                bOk = (boolean) importFn.invoke("B", dwlb.toString());
            } catch(IOException ioe) {}

            //var aOk = (boolean) importFn.invoke("A", dwla.toString());
            //var bOk = (boolean) importFn.invoke("B", dwlb.toString());
            System.out.println("Tried to import data: id=" + id + ", DWLA-OK? " + aOk + ", DWLB-OK? " + bOk);

            var results = context.getResults();
            results.setId(id);
            results.setSuccessA((boolean)aOk);
            results.setSuccessB((boolean)bOk);
            return READY_NOW;
        }
    }

    public static void main(String[] args) {
        if (args.length < 1) {
            return;
        }

        var hostPort = args[0].split(":");
        var address = new InetSocketAddress(hostPort[0], Integer.parseInt(hostPort[1]));
        try {
            var server = new EzRpcServer(new ImportService(), address);
            //var server = new EzRpcServer(new TestService(), address);
            var port = server.getPort();
            System.out.println("Listening on port " + port + "...");
            server.start().join();
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }
}
