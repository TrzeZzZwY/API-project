namespace WebApi.Dto.Input
{
    public class PublishTagInputDto
    {
        public string TagName { get; set; }

        public PublishTagInputDto(string tagName)
        {
            TagName = tagName;
        }
    }
}
