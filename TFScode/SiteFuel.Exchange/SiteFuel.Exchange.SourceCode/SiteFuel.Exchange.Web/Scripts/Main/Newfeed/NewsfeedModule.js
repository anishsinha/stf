var NewsfeedModule;
(function (NewsfeedModule) {
    var Newsfeed = /** @class */ (function () {
        function Newsfeed(data) {
            this.Message = data.FeedMessage;
            this.CreatedDate = data.CreatedDate;
            this.TargetUrl = data.TargetUrl;
        }
        Newsfeed.prototype.getHTML = function () {
            var html = '<div class="col-sm-12"><div class="border-b pb5"><h5 class="fs13">' + this.Message + '</h4>'
                + '<div class="fs11 color-grey"><i>On ' + this.CreatedDate + '</i></div></div></div>';
            return html;
        };
        return Newsfeed;
    }());
    NewsfeedModule.Newsfeed = Newsfeed;
    ;
    var Newsfeeds = /** @class */ (function () {
        function Newsfeeds() {
            this.feeds = new Array();
        }
        Newsfeeds.prototype.GetHTML = function () {
            var html = "";
            for (var i = 0; i < this.feeds.length; i++) {
                html = html + this.feeds[i].getHTML();
            }
            return html;
        };
        Newsfeeds.prototype.GetCurrentPage = function () {
            var page = $("#current-page").val();
            page = (page === undefined || page === "" ? "1" : page);
            return parseInt(page);
        };
        Newsfeeds.prototype.SetCurrentPage = function (number) {
            number = (number == undefined || number == null ? 1 : number);
            $("#current-page").val(number);
        };
        Newsfeeds.prototype.GetLatestId = function () {
            var id = $("#latestId").val();
            id = (id === undefined || id === "" ? "0" : id);
            return parseInt(id);
        };
        Newsfeeds.prototype.SetLatestId = function (id) {
            id = (id == undefined || id == null ? 1 : id);
            $("#latestId").val(id);
        };
        Newsfeeds.prototype.SetShowMore = function (targetUrl, entityId, containerId, totalPages, currentPage) {
            var showmore = $('[data-show-more]');
            if (totalPages > 0) {
                totalPages == currentPage ? showmore.hide() : showmore.show();
            }
        };
        Newsfeeds.prototype.ShowLatestNews = function (targetUrl, entityId, containerId) {
            var currentObject = this;
            $.get({
                data: { entityId: entityId, currentPage: 1, latestId: currentObject.GetLatestId() },
                url: targetUrl,
                success: function (data, textStatus, jqXHR) {
                    if (data.Messages != undefined) {
                        currentObject.feeds = new Array();
                        for (var i = 0; i < data.Messages.length; i++) {
                            var feed = new Newsfeed(data.Messages[i]);
                            currentObject.feeds.push(feed);
                        }
                        if (data.Messages.length > 0) {
                            $(containerId).html(currentObject.GetHTML() + $(containerId).html());
                            currentObject.SetLatestId(data.Messages[0].Id);
                        }
                    }
                    else if (data.indexOf('<title>TrueFill - Unauthorized Access</title>') >= 0) {
                        clearInterval(newsfeedInterval);
                    }
                },
                error: function (xhr) {
                    console.log(xhr);
                }
            });
        };
        Newsfeeds.prototype.LoadNews = function (targetUrl, entityId, containerId) {
            var currentObject = this;
            $.get({
                data: { entityId: entityId, currentPage: currentObject.GetCurrentPage() },
                url: targetUrl,
                success: function (data, textStatus, jqXHR) {
                    currentObject.feeds = new Array();
                    if (data.Messages != undefined) {
                        currentObject.SetCurrentPage(data.CurrentPage);
                        for (var i = 0; i < data.Messages.length; i++) {
                            var feed = new Newsfeed(data.Messages[i]);
                            currentObject.feeds.push(feed);
                        }
                        currentObject.SetShowMore(targetUrl, entityId, containerId, data.TotalPages, data.CurrentPage);
                        if (data.Messages.length > 0) {
                            $(containerId).html($(containerId).html() + currentObject.GetHTML());
                            currentObject.SetLatestId(data.Messages[0].Id);
                        }
                        $('#show-more-loading').hide();
                    }
                },
                error: function (xhr) {
                    console.log(xhr);
                    $('#show-more-loading').hide();
                }
            });
        };
        return Newsfeeds;
    }());
    NewsfeedModule.Newsfeeds = Newsfeeds;

    $(document).ready(function () {
        var newsfeeds = new NewsfeedModule.Newsfeeds();
        if (!isNaN(currentEntityId)) {
            newsfeeds.LoadNews(newsfeedUrl, currentEntityId, '#newsfeed-container');
            var showmore = $('[data-show-more]');
            showmore.click(function () {
                $('#show-more-loading').show();
                var news = new Newsfeeds(), page = news.GetCurrentPage();
                news.SetCurrentPage(page + 1);
                news.LoadNews(newsfeedUrl, currentEntityId, '#newsfeed-container');
            });
            var newsfeedInterval = newsfeeds.ShowLatestNews(newsfeedUrl, currentEntityId, '#newsfeed-container');
        }
    });
})(NewsfeedModule || (NewsfeedModule = {}));
//# sourceMappingURL=NewsfeedModule.js.map