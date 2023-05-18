namespace WebApi.Dto.Output
{
    public class PublishTagOutputDto
    {
        public string Name { get; set; }

        public PublishTagOutputDto(string name)
        {
            Name = name;
        }
    }
}
