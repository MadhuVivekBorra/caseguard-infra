using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Constructs;

namespace CaseGuardInfra
{
    public class NetworkStack : Stack
    {
        public Vpc Vpc { get; private set; }

        public NetworkStack(Construct scope, string id, StackProps props = null) : base(scope, id, props)
        {
            Vpc = new Vpc(this, "CaseGuardVpc", new VpcProps
            {
                MaxAzs = 2,
                NatGateways = 1
            });
        }
    }
}
