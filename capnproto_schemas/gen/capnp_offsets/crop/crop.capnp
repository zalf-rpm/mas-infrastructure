# crop.capnp
@0xf98a24e1969df972;
$import "/capnp/c++.capnp".namespace("mas::schema::crop");
$import "/capnp/go.capnp".package("crop");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/crop");
enum Cultivar @0xdd81b0520864e2b4 {
  alfalfaClovergrassLeyMix @0;
  alfalfa @1;
  bacharia @2;
  barleySpring @3;
  barleyWinter @4;
  cloverGrassLey @5;
  cottonBrMid @6;
  cottonLong @7;
  cottonMid @8;
  cottonShort @9;
  einkorn @10;
  emmer @11;
  fieldPea24 @12;
  fieldPea26 @13;
  grapevine @14;
  maizeGrain @15;
  maizeSilage @16;
  mustard @17;
  oatCompound @18;
  oilRadish @19;
  phacelia @20;
  potatoModeratelyEarly @21;
  rapeWinter @22;
  ryeGrass @23;
  ryeSilageWinter @24;
  ryeSpring @25;
  ryeWinter @26;
  sorghum @27;
  soybean0 @28;
  soybean00 @29;
  soybean000 @30;
  soybean0000 @31;
  soybeanI @32;
  soybeanII @33;
  soybeanIII @34;
  soybeanIV @35;
  soybeanV @36;
  soybeanVI @37;
  soybeanVII @38;
  soybeanVIII @39;
  soybeanIX @40;
  soybeanX @41;
  soybeanXI @42;
  soybeanXII @43;
  sudanGrass @44;
  sugarBeet @45;
  sugarcaneTransplant @46;
  sugarcaneRatoon @47;
  tomatoField @48;
  triticaleSpring @49;
  triticaleWinter @50;
  wheatDurum @51;
  wheatSpring @52;
  wheatWinter @53;
}
interface Crop @0xe88d97a324bf5c84 superclasses(import "/common.capnp".Identifiable) {
  cultivar @1 () -> (cult :Cultivar);
  parameters @0 () -> (params :AnyPointer);
}