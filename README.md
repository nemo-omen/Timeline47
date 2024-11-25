# Timeline47
Note: This application is in _very_ early development. The following is a rough outline of the project and is subject to change.

This application aims to provide a timeline of news events with links to subjects covered in those articles, along with overlays of economic data and other relevant information that will allow the user to better correlate events with publicly available data.

## The Plan
The plan is to create a web application that will:
1. Collect news articles from various sources using freely available RSS/Atom feeds.
2. Store metadata about the article, including date, title, url, and a short summary of the content.
3. Use Named Entity Recognition (NER) to identify subjects in the article.
4. Store metadata about the subjects, including the name, type, and a short summary of the subject.
5. Use the metadata to create a timeline of events and subjects.
6. Overlay economic, health, welfare, and other data, along with other relevant information on the timeline.

## Brief implementation details
The application uses a .NET backend that leverages FastEndpoints for the API and Entity Framework Core for the database. The frontend technology is not yet decided, but it will likely be a JavaScript framework like React or Svelte.

The data pipeline will be implemented in an event-driven fashion using a combination of FastEndpoint commands, events, and command handlers that will allow each small portion of the application to run when other parts of the application are finished. In effect, this creates a "daisy-chain" of events that will begin with news-gathering, proceed through processing individual articles with NER, fetch and store up-to-date data from various sources, and finally display the data in a timeline.

## Potential tools
### SLMs
- dslim/bert-base-NER
  - [Tutorials on Working with Hugging Face Models and Datasets](https://medium.com/@anyuanay/tutorials-on-working-with-hugging-face-models-and-datasets-a01dea1f1a81)
- distilbert-NER

### Interfacing
- ML.NET
- Python.NET
- Iron Python (might be easier to get values back from python)
- LLamaSharp
- Separate Python API

### Data
#### Economy
- US Bureau of Economic Analysis [BEA Data](https://www.bea.gov/data)
- US Bureau of Labor Statistics [BLS Data](https://www.bls.gov/data/)
- Federal Reserve Economic Data [FRED Data](https://fred.stlouisfed.org/)
- World Bank Data [WB Data](https://data.worldbank.org/)
- International Monetary Fund [IMF Data](https://www.imf.org/en/Data)
- Organisation for Economic Co-operation and Development [OECD Data](https://data.oecd.org/)
- United Nations [UN Data](https://data.un.org/)
- Eurostat [EU Data](https://ec.europa.eu/eurostat/data/database)
- International Labour Organization [ILO Data](https://www.ilo.org/global/statistics-and-databases/lang--en/index.htm)

#### Health
- World Health Organization [WHO Data](https://www.who.int/data/gho)

-- more data sources TBD