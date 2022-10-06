module BreadCrumbModule {

    declare var breadCrumbMappingData;
    declare var breadCrumbUrl: string;
    declare var homePageUrl: string;
    declare function GetSessionStorage(key: string): any;
    declare function SetSessionStorage(key: string, val: any): void;
    const bind_breadcrumb_data_key: string = "breadcrumbs";
    const breadcrumb_mapping_key = 'breadCrumbMapping';
    const home_page_key = 'Home';
    const home_page_url_key = 'dashboard';

    export class BreadCrumb {
        title: string;
        key: string;
        parents: string[];
        url: string;

        constructor(key: string, title: string, parents: string[], url: string) {
            this.key = key;
            this.title = title;
            this.parents = parents;
            this.url = url;
        }

        AddBreadCrumb(breadcrumb: BreadCrumb): void {
            var breadcrumbs = JSON.parse(GetSessionStorage(bind_breadcrumb_data_key));
            //when ever visiting home page clearing breadcrumb
            if (document.title.toLowerCase().indexOf(home_page_url_key) > 0 && breadcrumbs != null) {
                breadcrumbs = this.ClearBreadCrumbs();
                return;
            }

            var ismatching = false;
            if (breadcrumbs == null) {
                breadcrumbs = [];
                breadcrumbs.push(GetHomePage());
            }

            //clicking on any breadcrumb
            var matchingIndex = -1, matchingPosition = -1;
            breadcrumbs.some(function (el, i) {
                if (el.key == breadcrumb.key) {
                    matchingIndex = i;
                    return true;
                }
            });

            if (matchingIndex != -1) {
                breadcrumbs = this.RemoveBreadCrumb(matchingIndex);
            }

            if (breadcrumbs.length > 0) {
                ismatching = $.inArray(breadcrumbs[breadcrumbs.length - 1].key, breadcrumb.parents) != -1;
                if (!ismatching) //if current page parent matches in-between breadcrumb link removing crumbs which are after that
                {
                    $.each(breadcrumb.parents, function (ctr, value) {
                        $.inArray(value, breadcrumbs)
                        breadcrumbs.some(function (el, i) {
                            if (el.key == value && matchingPosition < i) {
                                matchingPosition = i;
                            }
                        });
                    });
                    if (matchingPosition != -1) {
                        ismatching = true;
                        breadcrumbs = this.RemoveBreadCrumb(matchingPosition + 1);
                    }
                }
            }

            // if parent is not matching clearing breadcrumbs , adding home page and then current page
            if (ismatching) {
                breadcrumbs.push(breadcrumb);
            }
            else {
                breadcrumbs = this.ClearBreadCrumbs();
                if (document.title.toLowerCase().indexOf(home_page_url_key) == -1)
                    breadcrumbs.push(breadcrumb);
            }
            SetSessionStorage(bind_breadcrumb_data_key, JSON.stringify(breadcrumbs));
        }

        ShowBreadCrumb(): void {
            var breadcrumbs = JSON.parse(GetSessionStorage(bind_breadcrumb_data_key));
            if (breadcrumbs.length == 1 && breadcrumbs[0].key == home_page_key) {
                return;
            }
            else {
                for (var i = 0; i < breadcrumbs.length; i++) {
                    var aTag = '<a href="' + breadcrumbs[i].url + '">' + breadcrumbs[i].title + '</a> <span> <i class="fa fa-angle-right mr5 ml5"></i> </span> ';
                    if (i == breadcrumbs.length - 1)
                        aTag = '<text>' + breadcrumbs[i].title + '</text>'
                    $('#breadcrumbs').append($(aTag));
                }
                $('#breadcrumbs').addClass("dib fs12 mb10 breadcrumb-wrapper")
            }
        }

        ClearBreadCrumbs(): BreadCrumb[] {
            var breadcrumb_arr = [];
            breadcrumb_arr.push(GetHomePage());
            SetSessionStorage(bind_breadcrumb_data_key, JSON.stringify(breadcrumb_arr));
            return breadcrumb_arr;
        }

        RemoveBreadCrumb(startindex: number): BreadCrumb[] {
            var breadcrumbs = JSON.parse(GetSessionStorage(bind_breadcrumb_data_key));
            if (breadcrumbs != null) {
                var removedCrumbs = breadcrumbs.splice(startindex, breadcrumbs.length - startindex);
                SetSessionStorage(bind_breadcrumb_data_key, JSON.stringify(breadcrumbs));
            }
            else {
                breadcrumbs = [];
            }
            return breadcrumbs;
        }
    }

    var breadCrumbMappingData;

    $(document).ready(function () {
        var breadCrumbMapping = GetSessionStorage(breadcrumb_mapping_key);
        if (breadCrumbMapping != null) {
            breadCrumbMappingData = JSON.parse(breadCrumbMapping);
            if (Array.isArray(breadCrumbMappingData)) {
                //defining breadcrumb
                var currentPage = breadCrumbMappingData.filter(function (item) { return item.page_title == document.title; });
                if (typeof (currentPage) != 'undefined' && currentPage != null && currentPage.length > 0) {
                    var breadcrumb = new BreadCrumbModule.BreadCrumb(currentPage[0].key, currentPage[0].breadcrumb_title, currentPage[0].parents, window.location.href);
                    breadcrumb.AddBreadCrumb(breadcrumb);
                    breadcrumb.ShowBreadCrumb();
                    return;
                }
            }
        }
        fetchBreadcrumbMappingData();
    });

    function fetchBreadcrumbMappingData() {
        $.get(breadCrumbUrl, function (response) {
            breadCrumbMappingData = response;
            SetSessionStorage(breadcrumb_mapping_key, JSON.stringify(response));
            var currentPage = breadCrumbMappingData.filter(function (item) { return item.page_title == document.title; });
            if (typeof (currentPage) != 'undefined' && currentPage != null && currentPage.length > 0) {
                var breadcrumb = new BreadCrumbModule.BreadCrumb(currentPage[0].key, currentPage[0].breadcrumb_title, currentPage[0].parents, window.location.href);
                breadcrumb.AddBreadCrumb(breadcrumb);
                breadcrumb.ShowBreadCrumb();
            }
        });
    }

    function GetHomePage() {
        var currentPage = breadCrumbMappingData.filter(function (item) { return item.key == home_page_key })[0];
        let breadcrumb = new BreadCrumbModule.BreadCrumb(currentPage.key, currentPage.breadcrumb_title, currentPage.parents, homePageUrl);

        return breadcrumb;
    }
}

