using Microsoft.Extensions.DependencyInjection;
using MauiApplication = Microsoft.Maui.Controls.Application;

namespace Taskia.Maui;

public partial class App : MauiApplication
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}