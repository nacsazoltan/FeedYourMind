namespace OkosDobozWeb.Models
{
    public class ExerciseItem
    {
        public string Topic { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ExerciseId { get; set; } = string.Empty;

        public int? Grade { get; set; }

        public string ThumbnailUrl
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ExerciseId) && ExerciseId.All(char.IsDigit))
                {
                    return $"https://tartalom.okosdoboz.hu/blobstorage/Uploads/Packages/Package-{ExerciseId}/thumbnail.png";
                }

                if (string.IsNullOrWhiteSpace(Topic))
                {
                    return string.Empty;
                }

                var topicKey = Topic.Replace('-', '_');
                return $"/images/thumb_{topicKey}.jpg";
            }
        }
    }
}
