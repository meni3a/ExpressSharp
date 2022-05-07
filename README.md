# ExpressSharp

An Express.js like NuGet package for building REST APIs using .NET

## Installation

You can install this package using the [NuGet]("https://www.nuget.org/") package
manager.

## How to use

Using c# 9 and above, you can take advantage of top-level statements capability, and use the code like that:

```cs
var app = new express();


app.use("/some", (req, res) => {
    Console.WriteLine("middleware");
});

app.get("/some", (req, res) => {
    res.status(200).send(req.query["someParam"]);
});

app.post("/index", (req, res) => {
    res.status(400).send(req.body);
});


app.listen(8070, () => {
    Console.WriteLine("server is listing..");
});

```

For other versions, just insert inside a function:

```cs
class Program
    {
        static void Main(string[] args)
        {
            var app = new express();

            
            app.use("/some", (req, res) => {
                Console.WriteLine("middleware");
            });

            app.get("/some", (req, res) => {
                res.status(200).send(req.query["someParam"]);
            });

            app.post("/index", (req, res) => {
                res.status(400).send(req.body);
            });


            app.listen(8070, () => {
                Console.WriteLine("server is listing..");
            });
            
            
            Console.ReadLine();
        }
    }
```

# License

[MIT]("https://choosealicense.com/licenses/mit/")
