using System;
using Braintree;

namespace SpaceShop_Utility.BrainTree
{
    public interface IBrainTreeBridge
    {
        IBraintreeGateway CreateGateWay();
        IBraintreeGateway GetGateWay();
    }
}
