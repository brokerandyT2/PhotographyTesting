namespace Location.Photography.UI
{
    public partial class AppPhotographyUI : Application
    {
        public AppPhotographyUI()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}