class Program
{
    static void Main(string[] args)
    {
        List<Video> videos = new List<Video>();

        Video breadVideo = new Video("How to Bake Bread", "Kitchen Basics", 480);
        breadVideo.AddComment(new Comment("Maria", "This recipe was easy to follow."));
        breadVideo.AddComment(new Comment("James", "My bread turned out great!"));
        breadVideo.AddComment(new Comment("Priya", "I will make this again."));
        videos.Add(breadVideo);

        Video drawingVideo = new Video("Drawing a Mountain Landscape", "Creative Corner", 615);
        drawingVideo.AddComment(new Comment("Leo", "The shading tips were very helpful."));
        drawingVideo.AddComment(new Comment("Ava", "Beautiful finished drawing!"));
        drawingVideo.AddComment(new Comment("Noah", "Please make more drawing videos."));
        videos.Add(drawingVideo);

        Video gardenVideo = new Video("Starting a Small Garden", "Green Home", 720);
        gardenVideo.AddComment(new Comment("Sofia", "I planted tomatoes after watching this."));
        gardenVideo.AddComment(new Comment("Ethan", "Thanks for explaining the soil mixture."));
        gardenVideo.AddComment(new Comment("Chloe", "This is perfect for beginners."));
        videos.Add(gardenVideo);

        Video guitarVideo = new Video("Your First Guitar Chords", "Music Steps", 540);
        guitarVideo.AddComment(new Comment("Daniel", "I can finally play these chords."));
        guitarVideo.AddComment(new Comment("Grace", "The camera angles made it easy to learn."));
        guitarVideo.AddComment(new Comment("Oliver", "A very clear lesson."));
        videos.Add(guitarVideo);

        foreach (Video video in videos)
        {
            Console.WriteLine($"Title: {video.GetTitle()}");
            Console.WriteLine($"Author: {video.GetAuthor()}");
            Console.WriteLine($"Length: {video.GetLength()} seconds");
            Console.WriteLine($"Number of comments: {video.GetCommentCount()}");
            Console.WriteLine("Comments:");

            foreach (Comment comment in video.GetComments())
            {
                Console.WriteLine($"- {comment.GetCommenterName()}: {comment.GetText()}");
            }

            Console.WriteLine();
        }
    }
}
