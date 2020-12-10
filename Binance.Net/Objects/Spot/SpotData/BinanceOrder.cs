﻿using System;
using System.Globalization;
using Binance.Net.Converters;
using Binance.Net.Enums;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Binance.Net.Objects.Spot.SpotData
{
    /// <summary>
    /// Information regarding a specific order
    /// </summary>
    public class BinanceOrder: ICommonOrder
    {
        /// <summary>
        /// The symbol the order is for
        /// </summary>
        public string Symbol { get; set; } = "";
        /// <summary>
        /// The order id generated by Binance
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// The order id as assigned by the client
        /// </summary>
        public string ClientOrderId { get; set; } = "";
        /// <summary>
        /// The order list id as generated by Binance, only for OCO orders
        /// </summary>
        public long OrderListId { get; set; }

        /// <summary>
        /// Original order id
        /// </summary>
        [JsonOptionalProperty]
        [JsonProperty("origClientOrderId")]
        public string OriginalClientOrderId { get; set; } = "";

        /// <summary>
        /// The price of the order
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The original quantity of the order
        /// </summary>
        [JsonProperty("origQty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The currently executed quantity of the order
        /// </summary>
        [JsonProperty("executedQty")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Cummulative amount
        /// </summary>
        [JsonProperty("cummulativeQuoteQty")]
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// The original quote order quantity
        /// </summary>
        [JsonProperty("origQuoteOrderQty")]
        public decimal QuoteQuantity { get; set; }
        /// <summary>
        /// The status of the order
        /// </summary>
        [JsonConverter(typeof(OrderStatusConverter))]
        public OrderStatus Status { get; set; }
        
        /// <summary>
        /// How long the order is active
        /// </summary>
        [JsonConverter(typeof(TimeInForceConverter))]
        public TimeInForce TimeInForce { get; set; }
        /// <summary>
        /// The type of the order
        /// </summary>
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType Type { get; set; }
        /// <summary>
        /// The side of the order
        /// </summary>
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// The stop price
        /// </summary>
        public decimal StopPrice { get; set; }
        /// <summary>
        /// The iceberg quantity
        /// </summary>
        [JsonProperty("icebergQty")]
        public decimal IcebergQuantity { get; set; }
        /// <summary>
        /// The time the order was submitted
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(TimestampConverter))]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// The time the order was last updated
        /// </summary>
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// Is working
        /// </summary>
        public bool IsWorking { get; set; }

        string ICommonOrderId.CommonId => OrderId.ToString(CultureInfo.InvariantCulture);
        string ICommonOrder.CommonSymbol => Symbol;
        decimal ICommonOrder.CommonPrice => Price;
        decimal ICommonOrder.CommonQuantity => Quantity;
        string ICommonOrder.CommonStatus => Status.ToString();
        bool ICommonOrder.IsActive => Status == OrderStatus.New || Status == OrderStatus.PartiallyFilled;

        IExchangeClient.OrderSide ICommonOrder.CommonSide =>
            Side == OrderSide.Sell ? IExchangeClient.OrderSide.Sell : IExchangeClient.OrderSide.Buy;

        IExchangeClient.OrderType ICommonOrder.CommonType
        {
            get
            {
                if (Type == OrderType.Limit)
                    return IExchangeClient.OrderType.Limit;
                if (Type == OrderType.Market)
                    return IExchangeClient.OrderType.Market;
                return IExchangeClient.OrderType.Other;
            }
        }
    }
}
