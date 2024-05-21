
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace SuperAniki.Laggen.Models
{
    public enum StaveTool
    {
        Curve,
        Front,
        End
    }
    public class Barrel
    {
        [JsonProperty("staveToolState")]
        public StaveTool StaveToolState { get; set; }

        [JsonProperty("staveCurveConfig")]
        public StaveCurveConfig? StaveCurveConfig { get; set; }

        [JsonProperty("staveEndConfig")]
        public StaveEndConfig? StaveEndConfig { get; set; }

        [JsonProperty("staveFrontConfig")]
        public StaveFrontConfig? StaveFrontConfig { get; set; }

        [JsonProperty("barrelDetails")]
        public BarrelDetails? BarrelDetails { get; set; }
    }

    public class BarrelDetails
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("notes")]
        public string? Notes { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("bottomDiameter")]
        public long BottomDiameter { get; set; }

        [JsonProperty("topDiameter")]
        public long TopDiameter { get; set; }

        [JsonProperty("staveLength")]
        public double StaveLength { get; set; }

        [JsonProperty("angle")]
        public double Angle { get; set; }

        [JsonProperty("staveBottomThickness")]
        public long StaveBottomThickness { get; set; }

        [JsonProperty("staveTopThickness")]
        public long StaveTopThickness { get; set; }

        [JsonProperty("bottomThickness")]
        public long BottomThickness { get; set; }

        [JsonProperty("bottomMargin")]
        public long BottomMargin { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }

        [JsonProperty("isExample")]
        public bool IsExample { get; set; }

        [JsonProperty("barrelId")]
        public string? BarrelId { get; set; }
    }

    public interface IStaveConfig
    {
        public string? Id { set; get; }
        public string? DefaultPaperType { set; get; }
        public string? BarrelId { set; get; }
        // public IConfigDetails[]? ConfigDetails { get; set; }
    }


    public interface IConfigDetails
    {
        public string? Id { get; set; }
        public string? PaperType { get; set; }
        public bool RotatePaper { get; set; }
    }

    public class StaveCurveConfig : IStaveConfig
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("defaultPaperType")]
        public string? DefaultPaperType { get; set; }

        [JsonProperty("barrelId")]
        public string? BarrelId { get; set; }

        [JsonProperty("configDetails")]
        public StaveCurveConfigDetail[]? ConfigDetails { get; set; }
    }

    public class StaveCurveConfigDetail : IConfigDetails
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("paperType")]
        public string? PaperType { get; set; }

        [JsonProperty("rotatePaper")]
        public bool RotatePaper { get; set; }

        [JsonProperty("posX")]
        public long PosX { get; set; }

        [JsonProperty("posY")]
        public long PosY { get; set; }

        [JsonProperty("innerTopX")]
        public double InnerTopX { get; set; }

        [JsonProperty("innerTopY")]
        public long InnerTopY { get; set; }

        [JsonProperty("outerTopX")]
        public double OuterTopX { get; set; }

        [JsonProperty("outerTopY")]
        public long OuterTopY { get; set; }

        [JsonProperty("innerBottomX")]
        public double InnerBottomX { get; set; }

        [JsonProperty("innerBottomY")]
        public long InnerBottomY { get; set; }

        [JsonProperty("outerBottomX")]
        public double OuterBottomX { get; set; }

        [JsonProperty("outerBottomY")]
        public long OuterBottomY { get; set; }

        [JsonProperty("rectX")]
        public long RectX { get; set; }

        [JsonProperty("rectY")]
        public long RectY { get; set; }

        [JsonProperty("rectWidth")]
        public long RectWidth { get; set; }

        [JsonProperty("rectHeight")]
        public long RectHeight { get; set; }

        [JsonProperty("staveCurveConfigId")]
        public string? StaveCurveConfigId { get; set; }
    }

    public class StaveEndConfig : IStaveConfig
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("defaultPaperType")]
        public string? DefaultPaperType { get; set; }

        [JsonProperty("barrelId")]
        public string? BarrelId { get; set; }

        [JsonProperty("configDetails")]
        public StaveEndConfigDetail[]? ConfigDetails { get; set; }
    }

    public class StaveEndConfigDetail : IConfigDetails
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("paperType")]
        public string? PaperType { get; set; }

        [JsonProperty("rotatePaper")]
        public bool RotatePaper { get; set; }

        [JsonProperty("topEndY")]
        public long TopEndY { get; set; }

        [JsonProperty("bottomEndY")]
        public long BottomEndY { get; set; }

        [JsonProperty("staveEndConfigId")]
        public string? StaveEndConfigId { get; set; }
    }

    public class StaveFrontConfig : IStaveConfig
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("defaultPaperType")]
        public string? DefaultPaperType { get; set; }

        [JsonProperty("barrelId")]
        public string? BarrelId { get; set; }

        [JsonProperty("configDetails")]
        public StaveFrontConfigDetail[]? ConfigDetails { get; set; }
    }

    public class StaveFrontConfigDetail : IConfigDetails
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("paperType")]
        public string? PaperType { get; set; }

        [JsonProperty("rotatePaper")]
        public bool RotatePaper { get; set; }

        [JsonProperty("posX")]
        public long PosX { get; set; }

        [JsonProperty("posY")]
        public long PosY { get; set; }

        [JsonProperty("spacing")]
        public long Spacing { get; set; }

        [JsonProperty("staveFrontConfigId")]
        public string? StaveFrontConfigId { get; set; }
    }

}