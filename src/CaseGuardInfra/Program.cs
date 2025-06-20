using Amazon.CDK;

namespace CaseGuardInfra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            var envUSEast = new Environment { Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"), Region = "us-east-1" };
            var envUSWest = new Environment { Account = System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"), Region = "us-west-2" };

            new NetworkStack(app, "NetworkStackUSEast", new StackProps { Env = envUSEast });
            new ComputeStack(app, "ComputeStackUSEast", new StackProps { Env = envUSEast });
            new DatabaseStack(app, "DatabaseStackUSEast", new StackProps { Env = envUSEast });

            new NetworkStack(app, "NetworkStackUSWest", new StackProps { Env = envUSWest });
            new ComputeStack(app, "ComputeStackUSWest", new StackProps { Env = envUSWest });
            new DatabaseStack(app, "DatabaseStackUSWest", new StackProps { Env = envUSWest });

            app.Synth();
        }
    }
}
