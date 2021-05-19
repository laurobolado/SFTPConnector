using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace SFTPConnector
{
    public class AWSS3Service
    {
        public AWSS3Service()
        {
        }

        public static async Task<ListBucketsResponse> ListingBucketsAsync()
        {
            using var s3Client = new AmazonS3Client();
            return await s3Client.ListBucketsAsync();
        }

        public static async Task<ListObjectsV2Response> ListingObjectsAsync(String bucketName)
        {
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                MaxKeys = 100
            };

            using var s3Client = new AmazonS3Client();
            return await s3Client.ListObjectsV2Async(request);
        }

        public static async Task<GetObjectResponse> GettingObjectAsync(String bucketName, String objectKey)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = objectKey
            };

            using var s3Client = new AmazonS3Client();
            return await s3Client.GetObjectAsync(request);
        }
    }
}
