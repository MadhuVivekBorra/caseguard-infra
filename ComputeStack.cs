using Amazon.CDK;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ECS.Patterns;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Logs;
using Constructs;

namespace CaseGuardInfra
{
    public class ComputeStack : Stack
    {
        public ComputeStack(Construct scope, string id, StackProps props = null) : base(scope, id, props)
        {
            var vpc = Vpc.FromLookup(this, "Vpc", new VpcLookupOptions { IsDefault = true });

            var cluster = new Cluster(this, "EcsCluster", new ClusterProps { Vpc = vpc });

            var taskRole = new Role(this, "TaskExecutionRole", new RoleProps
            {
                AssumedBy = new ServicePrincipal("ecs-tasks.amazonaws.com"),
                ManagedPolicies = new[] {
                    ManagedPolicy.FromAwsManagedPolicyName("service-role/AmazonECSTaskExecutionRolePolicy")
                }
            });

            new ApplicationLoadBalancedFargateService(this, "FargateService", new ApplicationLoadBalancedFargateServiceProps
            {
                Cluster = cluster,
                Cpu = 256,
                MemoryLimitMiB = 512,
                DesiredCount = 1,
                TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                {
                    Image = ContainerImage.FromRegistry("amazon/amazon-ecs-sample"),
                    ExecutionRole = taskRole,
                    LogDriver = LogDriver.AwsLogs(new AwsLogDriverProps
                    {
                        StreamPrefix = "web"
                    })
                },
                PublicLoadBalancer = true,
                AssignPublicIp = true 
            });
        }
    }
}
