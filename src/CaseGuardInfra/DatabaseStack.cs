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
            var vpc = Vpc.FromLookup(this, "Vpc", new VpcLookupOptions
            {
                IsDefault = true
            });

            var subnetGroup = new SubnetGroup(this, "RdsSubnetGroup", new SubnetGroupProps
            {
                Description = "Subnet group for RDS instance",
                Vpc = vpc,
                VpcSubnets = new SubnetSelection { SubnetType = SubnetType.PUBLIC },
                RemovalPolicy = RemovalPolicy.DESTROY
            });

            new DatabaseInstance(this, "RdsInstance", new DatabaseInstanceProps
            {
                Engine = DatabaseInstanceEngine.Postgres(new PostgresInstanceEngineProps
                {
                    // Downgraded to version 14, which is widely supported
                    Version = PostgresEngineVersion.VER_14
                }),
                InstanceType = InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.MICRO), // t3.micro is free tier eligible and supported
                Vpc = vpc,
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.PUBLIC
                },
                SubnetGroup = subnetGroup,
                Credentials = Credentials.FromGeneratedSecret("admin"),
                MultiAz = false,
                AllocatedStorage = 20,
                BackupRetention = Duration.Days(0),
                DeletionProtection = false,
                RemovalPolicy = RemovalPolicy.DESTROY
            });
        }
    }
}

