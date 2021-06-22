package de.zalf.mas;

import org.capnproto.*;
import org.junit.Assert;
import clojure.java.api.*;
import clojure.lang.IFn;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.channels.AsynchronousSocketChannel;
import java.util.concurrent.ExecutionException;

public class AClient {

    public static void usage() {
        System.out.println("usage: host:port");
    }

    public static void main(String[] args) {
        if (args.length < 1) {
            usage();
            return;
        }

        IFn require = Clojure.var("clojure.core", "require");
        require.invoke(Clojure.read("de.zalf.berest.core.datomic"));
        require.invoke(Clojure.read("datomic.api"));

        IFn currentDb = Clojure.var("de.zalf.berest.core.datomic", "current-db");
        var cd = currentDb.invoke("datomic:free://localhost:4334/berest/");

        IFn q = Clojure.var("datomic.api", "q");
        var out = q.invoke(Clojure.read("[:find ?se ?station-id :in $ :where [?se :weather-station/id ?station-id]]"), cd, "dwd_10162",
                Clojure.read("#inst \"2014-02-04T00:00:00.000-00:00\""));
        System.out.println(out);

        var endpoint = args[0].split(":");
        var address = new InetSocketAddress(endpoint[0], Integer.parseInt(endpoint[1]));
        try {
            var clientSocket = AsynchronousSocketChannel.open();
            clientSocket.connect(address).get();
            var rpcClient = new TwoPartyClient(clientSocket);
            var a = new de.zalf.mas.OuterA.A.Client(rpcClient.bootstrap());

            {
                System.out.println("Evaluating a literal...");
                var request = a.methodRequest();
                request.getParams().setParam("some_param");
                var prom = request.send();
                var response = rpcClient.runUntil(prom);
                var res = response.get();
                System.out.println(res.getRes());
            }
        }
        catch (IOException | InterruptedException | ExecutionException e) {
            e.printStackTrace();
        }
    }
}
