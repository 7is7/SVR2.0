namespace VueExample.Models.Charts.Plotly
{
    public class Box
    {
        [JsonProperty(PropertyName = "visible")]
        public bool Visible { get;set; }
        public Box()
        {
            Visible = true;
        }
    }
}