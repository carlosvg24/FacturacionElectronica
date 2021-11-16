<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadFact.aspx.cs" Inherits="FacturacionOnLine.DownloadFact" %>

<!DOCTYPE html>

<html lang="es-MX" xml:lang="es-MX">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>facturación.vivaaerobus.com</title>
    <script>
        (function (w, d, s, l, i) {
            w[l] = w[l] || []; w[l].push({
                'gtm.start': new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
                    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true;
            j.src = '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })
            (window, document, 'script', 'dataLayer', 'GTM-P457KB');
    </script>
   
</head>
<body>
    <noscript>
        <iframe src="//www.googletagmanager.com/ns.html?id=GTM-P457KB" height="0" width="0" style="display: none; visibility: hidden"></iframe>
    </noscript>
    <asp:HiddenField runat="server" ID="hfDownFac" />
</body>
</html>
