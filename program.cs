using _301289600Van_Lab1;

internal class Program
{
    static async Task Main(string[] args)
    {
        var s3Client = Helper.s3Client;
        var bucketOps = new BucketOps(s3Client);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== AWS S3 Bucket Manager =====");
            Console.WriteLine("1. List all buckets");
            Console.WriteLine("2. Create new bucket");
            Console.WriteLine("3. List objects in a bucket");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");

            var input = Console.ReadLine();
            if (input == "0") break;

            switch (input)
            {
                case "1":
                    await bucketOps.GetBucketListAsync();
                    break;
                case "2":
                    Console.Write("Enter new bucket name: ");
                    var bucketName = Console.ReadLine();
                    await bucketOps.CreateBucketAsync(bucketName ?? "");
                    break;
                case "3":
                    Console.Write("Enter bucket name to view objects: ");
                    var viewBucketName = Console.ReadLine();
                    await bucketOps.ListObjectsAsync(viewBucketName ?? "");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}
