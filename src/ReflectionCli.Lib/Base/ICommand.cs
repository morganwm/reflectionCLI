namespace ReflectionCli.Lib
{
    public interface ICommand
	{
		//this is going to trigger the major rewrite as I will need to modify all of the constructors to simply take in the assembly service
        //and variable service (if they need them) and all of the current constructors will need to be renamed "Run"
        //this is then going to change the way that the parser invokes them is it will no longer be getting constructor info but method info
        //and there will be no more activate, I will be 

        //needs to be revisited significantly

        //void Run();
        bool ExitVal();
    }
}