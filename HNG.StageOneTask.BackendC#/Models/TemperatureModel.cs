namespace HNG.StageOneTask.BackendC_.Models
{
    public class TemperatureModel
    {
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Temperature => $"{TemperatureC}�C / {TemperatureF}�F";
    }
}
