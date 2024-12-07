using Dollet.Core.Constants;

namespace Dollet.Helpers
{
    internal static class Defined
    {
        public static IEnumerable<string> Icons =>
        [
            MaterialDesignIcons.Account_balance,
            MaterialDesignIcons.Account_balance_wallet,
            MaterialDesignIcons.Wallet,
            MaterialDesignIcons.Savings,
            MaterialDesignIcons.Credit_card,
            MaterialDesignIcons.Paid,
            MaterialDesignIcons.Euro,
            MaterialDesignIcons.Wallet_giftcard,
            MaterialDesignIcons.Currency_exchange
        ];

        public static IEnumerable<string> Colors =>
        [
            "#d2b7b7", "#a76e6e", "#88af95", "#819a78", "#7782b0", "#606a9f", "#e39e83",
            "#855e5c", "#797d62", "#9b9b7a", "#ffcb69", "#f4a261", "#d08c60", "#997b66",
            "#e76f51"
        ];
    }
}