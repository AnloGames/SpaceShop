﻿using Braintree;
using LogicService.Service.IService;
using Microsoft.AspNetCore.Http;
using SpaceShop_Utility.BrainTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service
{
    public class PaymentService : IPaymentService
    {
        IBrainTreeBridge brainTreeBridge;

        public PaymentService(IBrainTreeBridge brainTreeBridge)
        {
            this.brainTreeBridge = brainTreeBridge;
        }

        public string GetTokenClient()
        {
            var getWay = brainTreeBridge.GetGateWay();
            var tokenClient = getWay.ClientToken.Generate();
            return tokenClient;
        }

        public string GetTransactionId(IFormCollection collection)
        {
            string nonce = collection["payment_method_nonce"];

            var request = new TransactionRequest
            {
                Amount = 1,
                PaymentMethodNonce = nonce,
                OrderId = "1",
                Options = new TransactionOptionsRequest { SubmitForSettlement = true }
            };

            var getWay = brainTreeBridge.GetGateWay();

            var resultTransaction = getWay.Transaction.Sale(request);

            var id = resultTransaction.Target.Id;
            var status = resultTransaction.Target.ProcessorResponseText;
            return id;
        }

        public void RefundTransaction(string transactionId)
        {
            var gateWay = brainTreeBridge.GetGateWay();

            // get transaction
            Transaction transaction = gateWay.Transaction.Find(transactionId);

            // условия при которых не возвращаем
            if (transaction.Status == TransactionStatus.AUTHORIZED ||
                transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
            {
                var res = gateWay.Transaction.Void(transactionId);
            }
            else // возврат средств
            {
                var res = gateWay.Transaction.Refund(transactionId);
            }
        }
    }
}
