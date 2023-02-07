---
layout: default
page_title: DEVELGUIDE
---

<div data-include-path="{{site.baseurl}}/site/TabBar.html"></div>

<main>

<div class="Side-Bar_dummy"></div>

<div class="h1-with-account">
  <h1 class="LargeTitle">디벨가이드</h1>
</div>

<div class="div-search mobile-search">
  <i class="iSearch iRegular"></i>
  <h3 class="Subheadline">테크 지식</h3>
</div>

<div class="Activity">
  <div class="text text-row">
    <h2 class="Title2">색인</h2>
  </div>

  <div class="category">
  {% assign pages_list = site.pages %}
  {% for node in pages_list %}
    {% if node.title != null %}
      {% if node.layout == "category" %}
      <a href="{{ site.baseurl }}{{ node.url }}">
        <div class="box-A {% if page.url == node.url %} active{% endif %}">
          <div class="box_text-A">
            <p class="Body f500">{{ node.title }}</p>
          </div>
          <div class="box_go">
            <i class="iArrowKeyRight fill"></i>
          </div>
        </div>
      </a>
      {% endif %}
    {% endif %}
  {% endfor %}
  </div>

  <div class="grid-row">
    <div class="box-A">
      <div class="box_text-A">
        <p class="Body f500">라이센스</p>
      </div>
      <div class="box_go">
        <i class="iArrowKeyRight fill"></i>
      </div>
    </div>
    <div class="box-A">
      <div class="box_text-A">
        <p class="Body f500">참여방법</p>
      </div>
      <div class="box_go">
        <i class="iArrowKeyRight fill"></i>
      </div>
    </div>
    <div class="box-A">
      <div class="box_text-A">
        <p class="Body f500">작성방법</p>
      </div>
      <div class="box_go">
        <i class="iArrowKeyRight fill"></i>
      </div>
    </div>
  </div>

</div>

</main>

<!-- Script pointing to jekyll-search.js -->
<script>history.scrollRestoration = "manual"</script>
<script type="text/javascript">
  window.addEventListener('scroll', function(e){
    let scroll = document.documentElement.scrollTop;
    if (scroll > 48) {
        document.querySelector('.PC-Header').classList.add('Fix');
        document.querySelector('.PC-Header_dummy').classList.remove('display-none');
    }
    else {
        document.querySelector('.PC-Header').classList.remove('Fix');
        document.querySelector('.PC-Header_dummy').classList.add('display-none');
    } 
  });
</script>
<script type="text/javascript" src="{{site.baseurl}}/asset/js/import.js"></script>
<script type="text/javascript" src="{{site.baseurl}}/asset/js/theme-toggle.js"></script>
<script type="text/javascript" src="{{site.baseurl}}/asset/js/search_ui.js"></script>
<script type="text/javascript" src="{{site.baseurl}}/asset/js/jekyll-search.min.js"></script>
<script type="text/javascript">
  SimpleJekyllSearch({
    searchInput: document.querySelector('.searchInput'),
    resultsContainer: document.querySelector('.searchResults'),
    json: '{{ site.baseurl }}/search.json',
    searchResultTemplate: '<a href="{url}" title="{desc}"><div class="box-A"><p class="Body f500">{title}</p></div></a>',
    noResultsText: '<div class="box-A search-none"><p class="Body f500">검색 결과를 찾을 수 없습니다..ㅠㅜ</p></div>',
    limit: 20,
    fuzzy: false,
    exclude: ['Welcome']
  })
</script>