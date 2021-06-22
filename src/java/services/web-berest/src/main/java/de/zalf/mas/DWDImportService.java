package de.zalf.mas;

import clojure.java.api.Clojure;
import clojure.lang.IFn;
import org.capnproto.CallContext;
import org.capnproto.EzRpcServer;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.util.concurrent.CompletableFuture;

public class DWDImportService {

    static class ImportService extends WebBerestDWDImport.DWLABImport.Server {

        private final IFn requireFn;
        private final IFn currentDbFn;
        private final IFn qFn;
        private final Object currentDb;
        private final String dbName;

        public ImportService() {
            requireFn = Clojure.var("clojure.core", "require");
            requireFn.invoke(Clojure.read("de.zalf.berest.core.datomic"));
            requireFn.invoke(Clojure.read("datomic.api"));

            qFn = Clojure.var("datomic.api", "q");
            currentDbFn = Clojure.var("de.zalf.berest.core.datomic", "current-db");

            dbName = System.getProperty("berest.datomic.url");
            currentDb = currentDbFn.invoke(dbName);
        }

        @Override
        protected CompletableFuture<Void> importData(CallContext<WebBerestDWDImport.DWLABImport.ImportDataParams.Reader, WebBerestDWDImport.DWLABImport.ImportDataResults.Builder> context) {
            var params = context.getParams();
            var dwla = params.getDwla();
            var dwlb = params.getDwlb();

            var out = qFn.invoke(Clojure.read("[:find ?se ?station-id :in $ :where [?se :weather-station/id ?station-id]]"),
                    currentDb, "dwd_10162", Clojure.read("#inst \"2014-02-04T00:00:00.000-00:00\""));
            System.out.println(out);

            var results = context.getResults();
            results.setSuccess(true);
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
            var port = server.getPort();
            System.out.println("Listening on port " + port + "...");
            server.start().join();
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }
}
