@0xa16af7a45e431f73;

using Go = import "/capnp/go.capnp";
$Go.package("vr");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/vr");

using Common = import "common.capnp";
using Funcs = import "functions.capnp";
#using Date = import "date.capnp".Date;
using Geo = import "geo.capnp";
using Registry = import "registry.capnp";

struct RT {

    using Cell2D = Geo.Point2D;
    # use a different name as we think in terms of cells (areas) not in terms of points
    # on any of the edges or the area

    struct RegionParams {
        # parameters specific to a subregion of a VR

        bounds @0 :Geo.RectBounds(Cell2D); 
        # bounds of region
        # the bounds are inclusive 
    }
    
    interface Region {
        #represents a region in a VR

        params @0 () -> RegionParams;
        # return the parameters for this region

        startCachingData @1 () -> (stop :Funcs.Action);
        # start caching registered data



    }

    struct VRParams {
        resolution @0 :Int64; # resolution in [m]
        regionParams @1 :RegionParams; # initial region parameters for the VR
    }

    interface Factory {
        # a factory creates VRs with the given set of parameters

        createVR @0 VRParams -> (vr :VR);
    }

    struct Data(T) {
        value @0 :T;
    }

    struct PrimData {
        value :union {
            int @0 :Int64;
            float @1 :Float64;
            bool @2 :Bool;
        }
    }

    interface DataAtCell(T) {
        # getting data at at a given cell
        # will only work for datasets created with a certain resolution and extend 

        getDataAtCell @0 Cell2D -> (data :T);
        # get data a certain cell
    }

    interface DataAtLatLonCoord(T) {
        # getting data at at a given coordinate
        # should work for all kinds of data as a coordinate is cell independent 

        getDataAtCoord @0 Geo.LatLonCoord -> (data :T);
        # get data a certain coordinate
    }


    interface DataService(T) extends(DataAtCell(T), DataAtLatLonCoord(T)) {
    }

    struct Model {}

    interface Cell2Coord2Cell {
        # interface to map between the fundamental cell coordinate space and some arbitrary geo coordinate space

        cell2Coord @0 Cell2D -> Geo.Coord;
        # map a cell coordinate to some geo-coordinate

        coord2Cell @1 Geo.Coord -> Cell2D;
        # map some geo-coordinate to a cell coordinate
    }


    interface VR extends(Region, Registry.Registry) {
        # capability to a VR 
        
        params @0 () -> VRParams;
        # get the parameters used to create this VR

        registerCoordMapping @4 (coordType :Geo.CoordType, func :Cell2Coord2Cell) -> (unregister :Funcs.Action);
        # register mapping functions

        registerDataService @1 (id :Text, data :DataService) -> (unregister :Funcs.Action);
        # a VR needs data and models to be registered, so they can be accessed
        # by users which hold a reference to the VR

        registerModel @2 (model :Model) -> (unregister :Funcs.Action);
        # register a model to use with this VR

        createSubRegion @3 RegionParams -> (unregister :Funcs.Action);
        # create a (sub-)region in this VR with the given parameters


    }

    


}