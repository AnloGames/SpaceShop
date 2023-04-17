using Braintree;
using Microsoft.Extensions.Options;
using ShopM4_Utility.BrainTree;
namespace SpaceShop_Utility.BrainTree
{
    public class BrainTreeBridge : IBrainTreeBridge
    {
        public SettingsBrainTree settingsBrainTree;
        public IBraintreeGateway gateway;

        public BrainTreeBridge(IOptions<SettingsBrainTree> options)
        {
            settingsBrainTree = options.Value;
        }   
        public IBraintreeGateway CreateGateWay()
        {
            return new BraintreeGateway(settingsBrainTree.Environment, settingsBrainTree.MerchantId,
                settingsBrainTree.PublicKey, settingsBrainTree.PrivateKey);
        }

        public IBraintreeGateway GetGateWay()
        {
            if (gateway == null)
            {
                gateway = CreateGateWay();
            }
            return gateway;
        }
        }
    }
