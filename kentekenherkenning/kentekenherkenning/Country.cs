namespace kentekenherkenning
{
    public struct Country
    {
        public string Name { get; set; }
        public int Characters { get; set; }
        public string TemplateLocation { get; set; }

        public Country(string name, int characters, string templateName)
        {
            Name = name;
            Characters = characters;
            TemplateLocation = $"../..//Templates/{templateName}.bin";
        }
    }
}