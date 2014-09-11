var g_FeedsEntriesPerSubitemCount = 5;

var g_devMediaTweetsLoading = false;
var g_devMediaTweetsLoaded = false;
var g_devBlogiFeedLoading = false;
var g_devBlogiFeedLoaded = false;
var g_devPytaniaFeedLoading = false;
var g_devPytaniaFeedLoaded = false;
var g_sysPytaniaFeedLoading = false;
var g_sysPytaniaFeedLoaded = false;
var g_antylamaFeedLoading = false;
var g_antylamaFeedLoaded = false;
var g_dotnetomaniakFeedLoading = false;
var g_dotnetomaniakFeedLoaded = false;
var g_97rzeczyFeedLoading = false;
var g_97rzeczyFeedLoaded = false;
var g_zenblogiFeedLoading = false;
var g_zenblogiFeedLoaded = false;
var g_mnmlzmFeedLoading = false;
var g_mnmlzmFeedLoaded = false;
var g_metadmFeedLoading = false;
var g_metadmFeedLoaded = false;

function onDevMediaTweetsLoaded(tweets) {
    g_devMediaTweetsLoading = false;
    g_devMediaTweetsLoaded = true;

    $('#devmedia-sidebar-devmedia-subitem .devmedia-sidebar-subitem-body')
      .html(createTweetsHtml(tweets));
}

function initializeSidebarEvents() {
    $('#devmedia-sidebar-devmedia-icon-container')
      .hover(function () {
          if (g_devMediaTweetsLoading || g_devMediaTweetsLoaded) {
              return;
          }

          g_devMediaTweetsLoading = true;
          g_devMediaTweetsLoaded = false;

          $.getJSON(
            'http://twitter.com/statuses/user_timeline/devMedia_pl.json?callback=?&count=5',
            onDevMediaTweetsLoaded);
      });

    $('#devmedia-sidebar-devblogi-icon-container')
      .hover(function () {
          if (g_devBlogiFeedLoading || g_devBlogiFeedLoaded) {
              return;
          }

          g_devBlogiFeedLoading = true;
          g_devBlogiFeedLoaded = false;

          loadFeedIntoContainer(
            'http://www.devblogi.pl/feeds/posts/default',
            'http://www.devblogi.pl',
            true,
            g_FeedsEntriesPerSubitemCount,
            $('#devmedia-sidebar-devblogi-subitem .devmedia-sidebar-subitem-body'),
            function () {
                g_devBlogiFeedLoading = false;
                g_devBlogiFeedLoaded = true;
            },
            function () {
                g_devBlogiFeedLoading = false;
                g_devBlogiFeedLoaded = false;
            }
          )
      });

    $('#devmedia-sidebar-devpytania-icon-container')
      .hover(function () {
          if (g_devPytaniaFeedLoading || g_devPytaniaFeedLoaded) {
              return;
          }

          g_devPytaniaFeedLoading = true;
          g_devPytaniaFeedLoaded = false;

          loadFeedIntoContainer(
            'http://devpytania.pl/feeds',
            'http://devpytania.pl',
            true,
            g_FeedsEntriesPerSubitemCount,
            $('#devmedia-sidebar-devpytania-subitem .devmedia-sidebar-subitem-body'),
            function () {
                g_devPytaniaFeedLoading = false;
                g_devPytaniaFeedLoaded = true;
            },
            function () {
                g_devPytaniaFeedLoading = false;
                g_devPytaniaFeedLoaded = false;
            }
          )
      });

    $('#devmedia-sidebar-syspytania-icon-container')
      .hover(function () {
          if (g_sysPytaniaFeedLoading || g_sysPytaniaFeedLoaded) {
              return;
          }

          g_sysPytaniaFeedLoading = true;
          g_sysPytaniaFeedLoaded = false;

          loadFeedIntoContainer(
            'http://syspytania.pl/feeds/rss',
            'http://syspytania.pl',
            true,
            g_FeedsEntriesPerSubitemCount,
            $('#devmedia-sidebar-syspytania-subitem .devmedia-sidebar-subitem-body'),
            function () {
                g_sysPytaniaFeedLoading = false;
                g_sysPytaniaFeedLoaded = true;
            },
            function () {
                g_sysPytaniaFeedLoading = false;
                g_sysPytaniaFeedLoaded = false;
            }
          )
      });

    $('#devmedia-sidebar-antylama-icon-container')
      .hover(function () {
          if (g_antylamaFeedLoading || g_antylamaFeedLoaded) {
              return;
          }

          g_antylamaFeedLoading = true;
          g_antylamaFeedLoaded = false;

          loadFeedIntoContainer(
            'http://antylama.pl/feeds',
            'http://antylama.pl',
            true,
            g_FeedsEntriesPerSubitemCount,
            $('#devmedia-sidebar-antylama-subitem .devmedia-sidebar-subitem-body'),
            function () {
                g_antylamaFeedLoading = false;
                g_antylamaFeedLoaded = true;
            },
            function () {
                g_antylamaFeedLoading = false;
                g_antylamaFeedLoaded = false;
            }
          )
      });

    $('#devmedia-sidebar-97rzeczy-icon-container')
      .hover(function () {
          if (g_97rzeczyFeedLoading || g_97rzeczyFeedLoaded) {
              return;
          }

          g_97rzeczyFeedLoading = true;
          g_97rzeczyFeedLoaded = false;

          loadFeedIntoContainer(
            'http://97rzeczy.devblogi.pl/rss/programisci',
            'http://97rzeczy.devblogi.pl',
            true,
            g_FeedsEntriesPerSubitemCount,
            $('#devmedia-sidebar-97rzeczy-subitem .devmedia-sidebar-subitem-body'),
            function () {
                g_97rzeczyFeedLoading = false;
                g_97rzeczyFeedLoaded = true;
            },
            function () {
                g_97rzeczyFeedLoading = false;
                g_97rzeczyFeedLoaded = false;
            }
          )
      });

    $('#devmedia-sidebar-dotnetomaniak-icon-container')
    .hover(function () {
        if (g_dotnetomaniakFeedLoading || g_dotnetomaniakFeedLoaded) {
            return;
        }

        g_dotnetomaniakFeedLoading = true;
        g_dotnetomaniakFeedLoaded = false;

        loadFeedIntoContainer(
          'http://feeds2.feedburner.com/Dotnetomaniakpl-OstatnioOpublikowaneArtykuy',
          'http://dotnetomaniak.pl',
          false,
          g_FeedsEntriesPerSubitemCount,
          $('#devmedia-sidebar-dotnetomaniak-subitem .devmedia-sidebar-subitem-body'),
          function () {
              g_dotnetomaniakFeedLoading = false;
              g_dotnetomaniakFeedLoaded = true;
          },
          function () {
              g_dotnetomaniakFeedLoading = false;
              g_dotnetomaniakFeedLoaded = false;
          }
        )
    });

    $('#devmedia-sidebar-zenblogi-icon-container')
    .hover(function () {
        if (g_zenblogiFeedLoading || g_zenblogiFeedLoaded) {
            return;
        }

        g_zenblogiFeedLoading = true;
        g_zenblogiFeedLoaded = false;

        loadFeedIntoContainer(
          'http://feeds.feedburner.com/zenblogi',
          'http://www.zenblogi.pl',
          false,
          g_FeedsEntriesPerSubitemCount,
          $('#devmedia-sidebar-zenblogi-subitem .devmedia-sidebar-subitem-body'),
          function () {
              g_zenblogiFeedLoading = false;
              g_zenblogiFeedLoaded = true;
          },
          function () {
              g_zenblogiFeedLoading = false;
              g_zenblogiFeedLoaded = false;
          }
        )
    });

    $('#devmedia-sidebar-mnmlzm-icon-container')
    .hover(function () {
        if (g_mnmlzmFeedLoading || g_mnmlzmFeedLoaded) {
            return;
        }

        g_mnmlzmFeedLoading = true;
        g_mnmlzmFeedLoaded = false;

        loadFeedIntoContainer(
          'http://feeds.feedburner.com/mnmlzm',
          'http://www.mnmlzm.pl',
          false,
          g_FeedsEntriesPerSubitemCount,
          $('#devmedia-sidebar-mnmlzm-subitem .devmedia-sidebar-subitem-body'),
          function () {
              g_mnmlzmFeedLoading = false;
              g_mnmlzmFeedLoaded = true;
          },
          function () {
              g_mnmlzmFeedLoading = false;
              g_mnmlzmFeedLoaded = false;
          }
        )
    });

    $('#devmedia-sidebar-metadm-icon-container')
    .hover(function () {
        if (g_metadmFeedLoading || g_metadmFeedLoaded) {
            return;
        }

        g_metadmFeedLoading = true;
        g_metadmFeedLoaded = false;

        loadFeedIntoContainer(
          'http://meta.devmedia.pl/feeds/rss',
          'http://meta.devmedia.pl',
          false,
          g_FeedsEntriesPerSubitemCount,
          $('#devmedia-sidebar-metadm-subitem .devmedia-sidebar-subitem-body'),
          function () {
              g_metadmFeedLoading = false;
              g_metadmFeedLoaded = true;
          },
          function () {
              g_metadmFeedLoading = false;
              g_metadmFeedLoaded = false;
          }
        )
    });
}

function createTweetsHtml(tweets) {
    var result = '';

    $.each(tweets, function () {
        var tweet = $(this)[0];

        var tweetStatus =
          tweet.text
            .replace(
              /((https?|s?ftp|ssh)\:\/\/[^"\s\<\>]*[^.,;'">\:\s\<\>\)\]\!])/g,
              function (url) {
                  return '<a href="' + url + '">' + url + '</a>';
              })
            .replace(
              /\B@([_a-z0-9]+)/ig,
              function (reply) {
                  return reply.charAt(0) + '<a href="http://twitter.com/' + reply.substring(1) + '">' + reply.substring(1) + '</a>';
              })
            .replace(
              /\B#([_a-z0-9]+)/ig,
              function (reply) {
                  return '<a href="http://twitter.com/search?q=%23' + reply.substring(1) + '">' + reply + '</a>';
              });

        result += '<div class="twitter-entry">';
        result += tweetStatus;
        result += '</div>';
    });

    return result;
}

function loadFeedIntoContainer(feedUrl, feedEntryBaseUrl, renderAuthor, maxEntriesCount, containerElem, loadSuccessCallback, loadFailureCallback) {
    var feed = new google.feeds.Feed(feedUrl);

    feed.setNumEntries(maxEntriesCount);

    feed.load(function (result) {
        if (result.error) {
            loadFailureCallback();
            return;
        }

        loadSuccessCallback();

        var feedsHtml = '';

        $.each(result.feed.entries, function () {
            var feedEntry = $(this)[0];
            var feedEntryLink = feedEntry.link;

            if (feedEntryLink.match('^http://') != 'http://') {
                feedEntryLink = feedEntryBaseUrl + feedEntryLink;
            }

            feedsHtml += '<div class="feed-entry">';
            feedsHtml += '  <span class="feed-entry-title">';
            feedsHtml += '    <a href="' + feedEntryLink + '">' + feedEntry.title + '</a>';
            feedsHtml += '  </span>';
            feedsHtml += '  <br />';
            if (renderAuthor) {
                feedsHtml += '  <span class="feed-entry-author">';
                feedsHtml += feedEntry.author;
                feedsHtml += '  </span>';
            }
            feedsHtml += '</div>';
        });

        containerElem.html(feedsHtml);
    });
}

function appendDevMediaSideBar(leftOrRight) {
    if (leftOrRight != 'left' && leftOrRight != 'right') {
        alert('devmedia.js: Argument leftOrRight should be equal to either "left" or "right".');
    }

    var sidebarHtml =
      '<div id="devmedia-sidebar" class="devmedia-sidebar-' + leftOrRight + '">' +
      '  <ul class="vertical-icons-list navmenu-v">' +
      '    <li id="devmedia-sidebar-devmedia-icon-container">' +
      '      <a href="http://www.devmedia.pl" target="_blank" title="devMedia" class="dm-sidebar-icon">' +
      '        <img src="~/Assets/Images/devmedia/ic_16x16_devMedia.png" alt="devMedia" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-devmedia-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Ostatnie wpisy na Twitterze <a href="https://twitter.com/devMedia_pl">devMedia_pl</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-devblogi-icon-container">' +
      '      <a href="http://www.devblogi.pl" target="_blank" title="devBlogi" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_devBlogi.png" alt="devBlogi" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-devblogi-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Najnowsze posty z serwisu <a href="http://www.devblogi.pl">devBlogi</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-devpytania-icon-container">' +
      '      <a href="http://devpytania.pl" target="_blank" title="devPytania" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_devPytania.png" alt="devPytania" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-devpytania-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Ciekawe <a href="http://devpytania.pl">devPytania</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-syspytania-icon-container">' +
      '      <a href="http://syspytania.pl" target="_blank" title="sysPytania" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_sysPytania.png" alt="sysPytania" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-syspytania-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Ciekawe <a href="http://syspytania.pl">sysPytania</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-antylama-icon-container">' +
      '      <a href="http://antylama.pl" target="_blank" title="antylama" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_antylama.png" alt="antylama" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-antylama-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Ciekawe pytania z serwisu <a href="http://antylama.pl">antylama</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-97rzeczy-icon-container">' +
      '      <a href="http://97rzeczy.devblogi.pl" target="_blank" title="97rzeczy" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_97rzeczy.png" alt="97rzeczy" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-97rzeczy-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Najnowsze posty z serwisu <a href="http://97rzeczy.devblogi.pl">97rzeczy</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-dotnetomaniak-icon-container">' +
      '      <a href="http://dotnetomaniak.pl" target="_blank" title="dotnetomaniak" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_dotnetomaniak.png" alt="dotnetomaniak" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-dotnetomaniak-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Najnowsze artykuły z serwisu <a href="http://dotnetomaniak.pl">dotnetomaniak</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-zenblogi-icon-container">' +
      '      <a href="http://www.zenblogi.pl" target="_blank" title="zenblogi" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_zenblogi.png" alt="zenblogi" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-zenblogi-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Najnowsze posty z serwisu <a href="http://www.zenblogi.pl">ZenBlogi</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-mnmlzm-icon-container">' +
      '      <a href="http://www.mnmlzm.pl" target="_blank" title="mnmlzm" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_mnmlzm.png" alt="mnmlzm" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-mnmlzm-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Najnowsze posty z serwisu <a href="http://www.mnmlzm.pl">mnmlzm</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '    <li id="devmedia-sidebar-metadm-icon-container">' +
      '      <a href="http://meta.devmedia.pl" target="_blank" title="meta devMedia" class="dm-sidebar-icon">' +
      '        <img src="http://www.devmedia.pl/static/gfx/logos/ic_16x16_metadm.png" alt="meta devMedia" />' +
      '      </a>' +
      '      <ul>' +
      '        <li>' +
      '          <div id="devmedia-sidebar-metadm-subitem" class="devmedia-sidebar-subitem">' +
      '            <h4>Ciekawe pytania z serwisu <a href="http://meta.devmedia.pl">meta.devMedia</a></h4>' +
      '            <div class="devmedia-sidebar-subitem-body">' +
      '              <div class="sidebar-loading-indicator">' +
      '                <img src="http://www.devmedia.pl/static/gfx/sidebar-loading-indicator.gif" alt="Wczytywanie danych ..." />' +
      '              </div>' +
      '            </div>' +
      '          </div>' +
      '        </li>' +
      '      </ul>' +
      '    </li>' +
      '  </ul>' +
      '</div>';

    $(sidebarHtml).prependTo('body');

    initializeSidebarEvents();
}

function trackEventAndRedirect(category, action, label, redirectUrl) {
    if (typeof (_gaq) != 'undefined') {
        _gaq.push(['_trackEvent', category, action, label]);
        setTimeout('document.location.href=\'' + redirectUrl + '\'', 100);
    }
}
