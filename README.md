### Parser MAC-address+
---
The Parser MAC-address+ program is designed to facilitate the automation of changing MAC addresses on network adapters based on data extracted from an HTML file generated by the Advanced IP Scanner program. This program leverages the HtmlAgilityPack library to parse the HTML file and extract MAC addresses, then utilizes PowerShell commands to manipulate network adapter settings.  


#### Features

1. MAC Address Manipulation: The program can change the MAC address of a specified network adapter to a new address obtained from the parsed HTML file.
2. HTML Parsing: It utilizes HtmlAgilityPack to parse an HTML file and extract MAC addresses from it.
3. Network Adapter Control: Utilizes PowerShell commands to interact with network adapters, enabling the program to set MAC addresses programmatically.
4. HTTP Request Handling: Sends HTTP GET requests to a specified URL and captures responses for logging and verification purposes.

A file of this type:  
![example.jpg](https://i.postimg.cc/jj2fZRbb/example.jpg)

#### How to Use

1. Replace the placeholder values in the program with actual network connection name, HTML file path, and URL.
2. Compile and execute the program.
3. The program will extract MAC addresses from the specified HTML file, change the MAC address of the network adapter to each extracted MAC address, send an HTTP GET request to the specified URL, and log the responses.
4. After completing the process, the program will revert the MAC address of the network adapter to its original value.
