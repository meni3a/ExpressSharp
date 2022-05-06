# ExpressSharp

An Express.js like nuget package for building REST APIs using .NET

## Installation

You can install this package using the [nuget]("https://www.nuget.org/") package
manager.

## How to use

An example using a simple console application

```cs
class Program
    {
        static void Main(string[] args)
        {
            var app = new express();

            app.use("/index", (req, res) =>
            {
                Console.WriteLine("middleware");
            });

            app.get("/index", (req, res) => {
                res.status(200).send("<HTML><BODY> Hello world!</BODY></HTML>");
            });

            app.post("/some", (req, res) => {
                res.status(400).send("<HTML><BODY>meniii!</BODY></HTML>");
            });

            app.listen(8070, () =>
            {
                Console.WriteLine("server is listing..");
            });
            Console.ReadLine();
        }
    }
```

# License

[MIT]("https://choosealicense.com/licenses/mit/")
