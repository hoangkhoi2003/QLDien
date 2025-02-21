using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;
using System.Globalization;
using System.Net;

namespace DemoQuanLyDienLuc
{
    public class VNPayTest
    {
        private readonly string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private readonly string vnp_TmnCode = "QL9R7JNY";
        private readonly string vnp_Version = "2.1.0";
        private readonly string vnp_Command = "pay";
        private readonly string vnp_CurrCode = "VND";
        private readonly string vnp_Locale = "vn";

        private readonly string vnp_HashSecret;

        public VNPayTest(string hashSecret)
        {
            vnp_HashSecret = hashSecret;
        }

        public string CreatePaymentUrl(string orderId, long amount, string orderInfo)
        {
            var urlParams = new Dictionary<string, string>
            {
                { "vnp_Version", vnp_Version },
                { "vnp_Command", vnp_Command },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", (amount * 100).ToString() },
                { "vnp_CurrCode", vnp_CurrCode },
                { "vnp_Locale", vnp_Locale },
                { "vnp_TxnRef", orderId },
                { "vnp_OrderInfo", orderInfo },
                { "vnp_OrderType", "billpayment" },
                { "vnp_ReturnUrl", "http://example.com/vnpay_return" },
                { "vnp_IpAddr", "127.0.0.1" },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };

            var rawHash = vnp_HashSecret + string.Join("", urlParams.OrderBy(x => x.Key).Select(x => x.Key + "=" + x.Value));
            var vnpSecureHash = HashByHmac(rawHash);

            urlParams.Add("vnp_SecureHash", vnpSecureHash);
            urlParams.Add("vnp_SecureHashType", "SHA256");

            return vnp_Url + "?" + string.Join("&", urlParams.Select(x => x.Key + "=" + HttpUtility.UrlEncode(x.Value)));
        }

        private string HashByHmac(string message)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(vnp_HashSecret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
        }
    }
}
