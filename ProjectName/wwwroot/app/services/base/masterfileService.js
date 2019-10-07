(function() {
    'use strict';

    define([], function () {
        return {
            masterfileService: function ($resource) {
                return {
                    Create: create,
                    Update: update,
                    DeleteById: deleteById,
                    CallWithUrl: callWithUrl,
                    CallGetMethodWithUrl: callGetMethodWithUrl
                };

                function create(model) {
                    return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/' + model + '/Create',
                            params: {

                            }
                        }
                    });
                }

                function update(model) {
                    return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: '/' + model + '/Update',
                            params: {

                            }
                        }
                    });
                }

                function deleteById(model) {
                    return $resource('', {},
                    {
                        perform: {
                            method: 'DELETE',
                            url: '/' + model + '/Delete',
                            params: {

                            }
                        }
                    });
                }

                function callWithUrl(url) {
                    return $resource('', {},
                    {
                        perform: {
                            method: 'POST',
                            url: url,
                            params: {

                            }
                        }
                    });
                }

                function callGetMethodWithUrl(url) {
                    return $resource('',
                        {},
                        {
                            perform: {
                                method: 'GET',
                                url: url,
                                params: {
                                }
                            }
                        });
                }
            }
        }
    });

}());