
## Adding an Index Pattern in Kibana

To visualize and query your data in Kibana, you need to create an index pattern that matches the indices in your Elasticsearch. Here’s how to add an index pattern through the Kibana UI:

### Step 1: Access Kibana

Open your web browser and go to the Kibana dashboard. If you're running locally, the URL is typically:

```
http://localhost:5601/app/management/kibana/indexPatterns/create
```

### Step 2: Navigate to Index Patterns

1. On the Kibana home page, click on the "Management" tab on the left sidebar.
2. Under "Kibana", click on "Index Patterns".

### Step 3: Create Index Pattern

1. Click on the "Create index pattern" button.
2. Enter the index pattern that matches the indices you want to explore. For example, if your indices are named like `identityservice-api-dev-2024-04`, you might use `identityservice-api-*` to match all indices starting with `identityservice-api-`.
3. Click "Next step".

### Step 4: Configure settings

1. Select the time field if your data contains time information. Commonly, this field is named `timestamp`.
2. Click on "Create index pattern".

### Step 5: Verify and Use

After creating the index pattern, you will be taken to a summary page showing all fields detected within the indices that match your pattern. You can now use this index pattern in Kibana to perform searches, create visualizations, and set up dashboards.

### Troubleshooting

If you do not see your data or receive an error, make sure that:
- The Elasticsearch indices you are targeting actually exist and contain data.
- The index name and pattern are correctly typed and match your naming conventions.
- Your user account has the appropriate permissions to view and create index patterns.

By following these steps, you can effectively set up and manage index patterns in Kibana, allowing for powerful data analysis and visualization tailored to your application's logging data.
