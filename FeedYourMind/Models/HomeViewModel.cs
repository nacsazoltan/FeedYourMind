using System.Collections.Generic;

namespace OkosDobozWeb.Models
{
    public class HomeViewModel
    {
        public string CurrentTopic { get; set; } = string.Empty;
        public List<TextItem> FilteredTexts { get; set; } = [];
        public List<VideoItem> FilteredVideos { get; set; } = [];
        public List<ExerciseItem> FilteredExercises { get; set; } = [];
    }
}