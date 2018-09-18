<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TwitterFeedControl.ascx.vb" Inherits="CloudCar.CCControls.Blogging.TwitterFeedControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<script src="http://widgets.twimg.com/j/2/widget.js" type="text/javascript"></script>
<script type="text/javascript">
new TWTR.Widget({version: 2, type: 'profile', rpp: 3, interval: 6000, width: 230, height: 300, theme: {
    shell: { background: '#333', color: '#fff'},
    tweets: { background: '#fff', color: '#333', links: '#BF0000'}
  },
  features: { scrollbar: false, loop: false, live: false, hashtags: true, timestamp: true, avatars: false, behavior: 'all'}
}).render().setUser('<%= Settings.TwitterUser %>').start();
</script>
