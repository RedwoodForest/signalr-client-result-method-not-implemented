namespace Server {
    public class Program {
        public static void Main(string[] args) {
            var app = AppBuilder.Build(args);
            app.Run();
        }
    }
}