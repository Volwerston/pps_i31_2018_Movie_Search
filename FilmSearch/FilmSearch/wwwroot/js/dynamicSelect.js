function defaultSelectAjax(dataUrl, mapper) {
    return {
        url: dataUrl,
        data: function(params) {
            return {
                q: params.term || "",
                page: params.page || 1
            }
        },
        processResults: function(result, params) {
            params.page = params.page || 1;

            return {
                results: result.data.map(mapper),
                pagination: {
                    more: (result.pageSize * params.page) < (result.totalCount)
                }
            }
        },
        cache: true,
        delay: 500,
        dataType: 'json',
        type: 'GET'
    }
}