function loadWidgetQuote(divname, symbol, width, colorTheme, isTransparent, locale) {
    try {
        var widgetContainer = document.createElement('div');
        widgetContainer.className = 'tradingview-widget-container';

        var widget = document.createElement('div');
        widget.className = 'tradingview-widget-container__widget';
        widgetContainer.appendChild(widget);

        var copyright = document.createElement('div');
        copyright.className = 'tradingview-widget-copyright';

        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = 'https://s3.tradingview.com/external-embedding/embed-widget-single-quote.js';
        script.async = true;
        script.innerHTML = JSON.stringify({
            "symbol": symbol,
            "width": width,
            "colorTheme": colorTheme,
            "isTransparent": isTransparent,
            "locale": locale
        });
        copyright.appendChild(script);

        var webViewContainer = document.getElementById(divname);
        if (webViewContainer) {
            webViewContainer.innerHTML = '';
            webViewContainer.appendChild(widgetContainer);
            webViewContainer.appendChild(copyright);
        }
    }
    catch (error) {
        var errorMessage = document.createElement('div');
        errorMessage.textContent = 'Произошла ошибка: ' + error.message;
        webViewContainer.appendChild(errorMessage);
    }
}

function loadWidgetGraph(divname, symbol, width, height, theme, locale) {
    var widgetContainer = document.createElement('div');
    widgetContainer.className = 'tradingview-widget-container';

    var tradingviewDiv = document.createElement('div');
    tradingviewDiv.id = 'tradingview_b05b8';
    widgetContainer.appendChild(tradingviewDiv);

    var copyright = document.createElement('div');
    copyright.className = 'tradingview-widget-copyright';

    var link = document.createElement('a');
    link.href = 'https://ru.tradingview.com/';
    link.rel = 'noopener nofollow';
    link.target = '_blank';
    copyright.appendChild(link);

    var script1 = document.createElement('script');
    script1.type = 'text/javascript';
    script1.src = 'https://s3.tradingview.com/tv.js';
    script1.async = true;

    var script2 = document.createElement('script');
    script2.type = 'text/javascript';
    var widgetSettings = {
        "width": width,
        "height": height,
        "symbol": symbol,
        "interval": "D",
        "timezone": "Etc/UTC",
        "theme": theme,
        "style": "1",
        "locale": locale,
        "toolbar_bg": "#f1f3f6",
        "enable_publishing": false,
        "allow_symbol_change": true,
        "container_id": "tradingview_b05b8"
    };

    script1.onload = function () {
        // Когда скрипт tv.js загружен и объект TradingView определен
        script2.innerHTML = 'new TradingView.widget(' + JSON.stringify(widgetSettings) + ');';

        var webViewContainer = document.getElementById(divname);
        if (webViewContainer) {
            webViewContainer.innerHTML = '';
            webViewContainer.appendChild(widgetContainer);
            webViewContainer.appendChild(script2);
            webViewContainer.appendChild(script1); // Помещаем script1 после script2, чтобы он был в DOM перед tv.js
            webViewContainer.appendChild(script1);
            webViewContainer.appendChild(script2);
            webViewContainer.appendChild(script1); // Помещаем script1 после script2, чтобы он был в DOM перед tv.js
            webViewContainer.appendChild(script2);
        }
    };

    var webViewContainer = document.getElementById(divname);
    if (webViewContainer) {
        webViewContainer.innerHTML = '';
        webViewContainer.appendChild(widgetContainer);
        webViewContainer.appendChild(script1);
    }
}