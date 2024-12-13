
using System.Security.Cryptography;
using System.Text;

namespace DistributedLogging.Presistence.AmazonS3StorgeService
{
    public class S3StorageService
    {
        /// <summary>
        /// my own Implemntation for S3 protocol but I do not have credentials it needs amazon account
        /// </summary>
        /// 
        private static readonly HttpClient httpClient = new HttpClient();
        private const string ServiceUrl = "https://s3.example.com";
        private const string BucketName = "my-logs";
        private const string AccessKey = "your-access-key";
        private const string SecretKey = "your-secret-key";
        private const string Region = "us-east-1"; // Replace with your region

        public async Task UploadFile(string fileName, string fileContent)
        {
            // Step 1: Define the PUT request URL
            string objectUrl = $"{ServiceUrl}/{BucketName}/{fileName}";

            // Step 2: Create the authorization header
            string date = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
            string credentialScope = $"{date.Substring(0, 8)}/{Region}/s3/aws4_request";

            string signedHeaders = "host;x-amz-date";
            string canonicalRequest = $"PUT\n/{BucketName}/{fileName}\n\nhost:{ServiceUrl}\nx-amz-date:{date}\n\n{signedHeaders}\nUNSIGNED-PAYLOAD";

            string stringToSign = $"AWS4-HMAC-SHA256\n{date}\n{credentialScope}\n{Hash(canonicalRequest)}";
            byte[] signingKey = GetSignatureKey(SecretKey, date.Substring(0, 8), Region, "s3");
            string signature = ToHexString(HmacSHA256(stringToSign, signingKey));

            string authorizationHeader = $"AWS4-HMAC-SHA256 Credential={AccessKey}/{credentialScope}, SignedHeaders={signedHeaders}, Signature={signature}";

            // Step 3: Send the PUT request
            var request = new HttpRequestMessage(HttpMethod.Put, objectUrl);
            request.Headers.Add("Authorization", authorizationHeader);
            request.Headers.Add("x-amz-date", date);
            request.Content = new StringContent(fileContent, Encoding.UTF8, "text/plain");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("File uploaded successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to upload file: {response.StatusCode}");
            }
        }

        private static string Hash(string text)
        {
            using var sha256 = SHA256.Create();
            return ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        private static byte[] GetSignatureKey(string secretKey, string date, string region, string service)
        {
            byte[] kDate = HmacSHA256(date, Encoding.UTF8.GetBytes("AWS4" + secretKey));
            byte[] kRegion = HmacSHA256(region, kDate);
            byte[] kService = HmacSHA256(service, kRegion);
            return HmacSHA256("aws4_request", kService);
        }

        private static byte[] HmacSHA256(string data, byte[] key)
        {
            using var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private static string ToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
