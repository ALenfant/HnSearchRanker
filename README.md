# HnSearchRanker

## Prerequisites
This project is coded in .Net Core 2.1 and requires it to be ran.

## Usage
To start the project, do the following in the same folder as this README:

```
dotnet restore
cd HnSearchRanker
dotnet run
```

You will then be able to access the server on the port 5000 (http) and 5001 (https). For example, `https://localhost:5001/1/queries/count/2015`.

* `GET /1/queries/count/<DATE_PREFIX>`: returns a JSON object specifying the number of distinct queries that have been done during a specific time range
* `GET /1/queries/popular/<DATE_PREFIX>?size=<SIZE>`: returns a JSON object listing the top <SIZE> popular queries that have been done during a specific time range

### Examples
* Distinct queries done in 2015: `GET /1/queries/count/2015`
* Distinct queries done in Aug: `GET /1/queries/count/2015-08`
* Distinct queries done on Aug 3rd: `GET /1/queries/count/2015-08-03`
* Distinct queries done on Aug 1st between 00:04:00 and 00:04:59: `GET /1/queries/count/2015-08-01 00:04`
* Top 3 popular queries done in 2015: `GET /1/queries/popular/2015?size=3`
* Top 5 popular queries done on Aug 2nd: `GET /1/queries/popular/2015-08-02?size=5`

## Tests
There is a battery of unit and integration tests included. To run them, do the following in the same folder as this README:

```
dotnet restore
dotnet test
```

## How it works
### Parsing
#### Parsing date prefixes (DatePrefixParser.cs)
To read a date prefix, we first parse it into a DateTime by adding all the eventually missing data from a template.

Then, we need to know the unit of time to add in order to create our range of DateTimes. To do so, we check the length of the provided date prefix and add
the unit of time corresponding to it.

#### Parsing logs (LogParser.cs)
The parsing reads the whole HN archive file provided (`hn_logs.tsv`) and parses its DateTimes and queries.

The various DateTimes are inserted in a SortedSet, which is a self-balancing tree provided by the .NET Framework.
Since we're given no guarantees that the logs are provided in sorted order, this allows us to ensure that they are always sorted, with an insert complexity of `O(logn)`.

We also insert the queries in a Dictionary from DateTime to the list of queries performed at this instant.

### Querying (HnDataRepository.cs)
This structure constains the SortedSet and the Dictionary.

When we want to make any query on the data (whether distinct queries counting or most popular ones), we always use the GetQueriesBetween method.
This method gets the queries between two DateTimes (in order to not include the second DateTimes, its value is reduced by the smallest unit possible, a Tick).
To do so, it uses the SortedSet's built-in GetViewBetween method, which can find both ends in `O(logn)` time and return a view of the notes between them. Due
to .NET Framework's requirement that the Count property always being `O(1)`, it however also goes over all the K found elements to update it, which makes it
`O(K)`.

#### Distinct queries count
To count distinct queries, we just use a HashSet to have amortized `O(1)` insertion and retrieval. Then, we add all the queries we found in the time window one
by one to the set and return its Count property.

#### Most popular queries
To return the most popular queries, we use a Dictionary to have amortized `O(1)` insertion and retrieval. Then, we go over all the queries and use the
Dictionary to count the number of times we see each of them.

Finally, we sort the Dictionary's entries by value in descending order, which is `O(nlogn)` and we return the K most popular ones as requested.

## Technical Choices
I chose to use .NET Core because I'm very familiar with C# and how to use it. This also allowed the project to be cross-platform, which was something that was
important for me, as I currently code on Windows but wanted the code to be executable by anyone.

The use of ASP.Net Core was also for simplicity's sake, as it's very easy to create a REST API with it that serializes automatically objects to JSON and where
the routing is easy to understand.