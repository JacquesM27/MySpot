namespace MySpot.Tests.Integration.Controllers
{
    public abstract class ControllerTestsBase
    {
        internal MySpotTestApp _app;
        protected HttpClient Client { get; }

        public ControllerTestsBase()
        {
            _app = new MySpotTestApp();
            Client = _app.Client;
        }
    }
}
