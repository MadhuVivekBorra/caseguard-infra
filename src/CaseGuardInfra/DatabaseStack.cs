using Amazon.CDK;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.EC2;
using Constructs;

namespace CaseGuardInfra
{
    public class DatabaseStack : Stack
    {
        public DatabaseStack(Construct scope, string id, StackProps props = null) : base(scope, id, props)
        {
            var vpc = Vpc.FromLookup(this, "Vpc", new VpcLookupOptions { IsDefault = true });

            new DatabaseInstance(this, "RdsInstance", new DatabaseInstanceProps
            {
                Engine = DatabaseInstanceEngine.Postgres(new PostgresInstanceEngineProps
                {
                    Version = PostgresEngineVersion.VER_15
                }),
                Vpc = vpc,
                InstanceType = Amazon.CDK.AWS.EC2.InstanceType.Of(InstanceClass.BURSTABLE2, InstanceSize.SMALL),
                Credentials = Credentials.FromGeneratedSecret("admin"),
                MultiAz = false,
                AllocatedStorage = 20,
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.PUBLIC
                },
                RemovalPolicy = RemovalPolicy.DESTROY
            });
        }
    }
}
