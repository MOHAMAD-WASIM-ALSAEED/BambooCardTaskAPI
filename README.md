# Bamboo Task API

## Description

This project is an API designed to fetch the top N best stories from Hacker News API based on their scores in descending order. It provides endpoints to retrieve these stories, with features including rate limiting, retry policy, circuit breaker policy, and memory caching to enhance performance, resilience, and protection against abuse.

## Endpoints

- **GET /BestStories**
  - Summary: Get the top N best stories from Hacker News.
  - Parameters:
    - `count`: (Optional) Number of stories to retrieve (default is all stories). Must be greater than 1.
  - Responses:
    - 200 OK: Returns the top N best stories.
    - 400 Bad Request: Invalid request parameters.
    - 429 Too Many Requests: Rate limit exceeded.
    - 500 Internal Server Error: Server error occurred.

- **Health Check**
  - Path: `/healthy`
  - Summary: Check the health status of the API.

## Features

- **Rate Limiter:** Limits each IP address to 5 requests per minute.
  
- **Retry Policy:** Retries failed requests up to 4 times with a 2-second delay between attempts.

- **Circuit Breaker Policy:** Breaks the circuit for 60 seconds after 12 consecutive failures within that timeframe.

- **Memory Cache:** Stores responses in memory for 1 minute to improve performance.

- **Swagger UI:** Provides interactive documentation and exploration of the API's capabilities.

## Dependencies

- Checks.UI
- AutoMapper
- Polly
- Swagger UI

## Usage

To use the API, send HTTP requests to the specified endpoints using the following base URL: https://localhost:7161/HackerNews


Ensure that request parameters are correctly formatted, and handle responses according to the provided status codes.

### Example Request-Response

**Request:**
GET /HackerNews/BestStories?count=2

**Response:**
```json
[
    {
        "title": "I designed a cube that balances itself on a corner",
        "uri": "https://willempennings.nl/balancing-cube/",
        "postedBy": "dutchkiwifruit",
        "time": "2024-02-11T16:36:58+00:00",
        "score": 2429,
        "commentCount": 374
    },
    {
        "title": "Cloudflare defeats patent troll Sable at trial",
        "uri": "https://blog.cloudflare.com/cloudflare-defeats-patent-troll-sable-at-trial",
        "postedBy": "jgrahamc",
        "time": "2024-02-12T14:14:42+00:00",
        "score": 1045,
        "commentCount": 407
    }
]
```
**Request:**
GET /HackerNews/BestStories
**Response:**
```json
[
 All best stories
]
```
## Future Improvements Planned for V2

### Features to Add

1. **Distributed Cache Integration:** Implement a distributed cache like Redis to improve scalability and resilience.
2. **Security Enhancement with JWT Token:** Introduce JWT token-based authentication and authorization for improved security.

### Architecture Modifications

1. **Containerization with Docker:** Package the application as a Docker image for easier deployment.
2. **Container Orchestration with Kubernetes:** Enable container orchestration using Kubernetes to automate deployment, scaling, and load balancing.

## Author

Mohamad Wasim Alsaeed

