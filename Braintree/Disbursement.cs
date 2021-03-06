using System;
using System.Collections;
using System.Collections.Generic;

namespace Braintree
{
    public class Disbursement
    {
        public string Id { get; protected set; }
        public decimal? Amount { get; protected set; }
        public string ExceptionMessage { get; protected set; }
        public DateTime? DisbursementDate { get; protected set; }
        public string FollowUpAction { get; protected set; }
        public MerchantAccount MerchantAccount { get; protected set; }
        public List<string> TransactionIds { get; protected set; }
        public bool? Success { get; protected set; }
        public bool? Retry { get; protected set; }

        private BraintreeGateway gateway;

        public Disbursement(NodeWrapper node, BraintreeGateway gateway)
        {
            Id = node.GetString("id");
            Amount = node.GetDecimal("amount");
            ExceptionMessage = node.GetString("exception-message");
            DisbursementDate = node.GetDateTime("disbursement-date");
            FollowUpAction = node.GetString("follow-up-action");
            MerchantAccount = new MerchantAccount(node.GetNode("merchant-account"));
            TransactionIds = new List<string>();
            foreach (var stringNode in node.GetList("transaction-ids/item")) 
            {
                TransactionIds.Add(stringNode.GetString("."));
            }
            Success = node.GetBoolean("success");
            Retry = node.GetBoolean("retry");
            this.gateway = gateway;
        }

        public ResourceCollection<Transaction> Transactions()
        {
            var gateway = new TransactionGateway(this.gateway);

            var searchRequest = new TransactionSearchRequest().
                Ids.IncludedIn(TransactionIds.ToArray());

            return gateway.Search(searchRequest);
        }
    }
}
