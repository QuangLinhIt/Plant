function Paging(currentPage, lastpage) {

    if (lastpage == 0) {
        return "";
    }
    if (lastpage == 1) {
        return "";
    }
    var block = 6;
    //var currentPage = 1;
    var nextPage = currentPage + 1;
    var perPage = currentPage - 1;
    var tempcurrentPage = currentPage + parseInt(currentPage / block);
    var currentBlock = parseInt(tempcurrentPage / block);
    var startPage = 0;
    var endPage = 0;
    var li = "";
    var extendli = "";

    if (currentBlock === 0) {
        startPage = 1;
        endPage = 6;


    } else {
        endPage = block * (currentBlock + 1) - currentBlock;
        startPage = endPage - 5;
    }
    if (endPage > lastpage) {
        endPage = lastpage;
    }
    for (let i = startPage; i <= endPage; i++) {
        li += `<li class="page-item ${i === currentPage ? "active" : ""} "><a class="page-link"  onclick=Search(${i})>${i}</a></li>`;
    }
    //console.log(currentBlock);
    var endBlock = parseInt(lastpage / block);
    var a = lastpage - endBlock * block;
    if (a > 0) {
        endBlock = endBlock + 1;
    }
    //console.log(endBlock);
    if (endBlock-1 > currentBlock) {
        extendli = ` <li class="page-item"><a class="page-link" href="?page=1">...</a></li>
                     <li class="page-item"><a class="page-link" onclick=Search(${lastpage})>${lastpage}</a></li>`;
    }
    //console.log(lastpage);
    //console.log(block);
    //console.log(currentBlock);
    //console.log(endBlock);



    var a = ` <li class="page-item"><a class="page-link" href="#">...</a></li>
              <li class="page-item"><a class="page-link" href="#">15</a></li>`;

    var htmlPaging = `
                       <nav class="pagination-wrap d-inline-block" aria-label="navigation">
                          <ul class="pagination ">

                              <li  class="page-item">
                              <a class="page-link" ${perPage !== 0 ? "onclick=Search(" + perPage + ")" : ""}  aria-label="Previous">
                                                    <span aria-hidden="true">«</span>
                                                    <span class="sr-only">Previous</span>
                              </a>
                              </li>
                             ${li}
                             ${currentBlock < endBlock? extendli:""}
                              <li class="page-item">
                               <a ${nextPage <= lastpage ? "onclick=Search(" + nextPage + ")" : ""} class="page-link" aria-label="Next">
                                                    <span aria-hidden="true">»</span>
                                                    <span class="sr-only">Next</span>
                               </a>
                            </li>
                          </ul>
                       </nav>`;
    return htmlPaging;
}