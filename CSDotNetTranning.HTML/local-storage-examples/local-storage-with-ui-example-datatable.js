const tblBlog = 'Tbl_Blog';
let _blogId = '';
const readBlog = () => {
    if ($.fn.DataTable.isDataTable('#datatable')) {
        $('#datatable').DataTable().destroy();
    }
    $('#tbDataTable').html('');

    let lstBlog = getBlogs();

    let htmlRow = "";

    lstBlog.forEach((item, index) => {
        htmlRow += `
        <tr>
            <td>
                <button type="button" class="btn btn-warning" onclick="editBlog('${item.Id}')">Edit</button>
                <button type="button" class="btn btn-danger" onclick="deleteBlog('${item.Id}')">Delete</button>
            </td>
            <th scope="row">${index + 1}</th>
            <td>${item.Title}</td>
            <td>${item.Author}</td>
            <td>${item.Content}</td>
        </tr>
        `
    });
    $('#tbDataTable').html(htmlRow);

    new DataTable('#datatable');
}

const getBlogs = () => {
    let lstBlogs = [];
    let blogStr = localStorage.getItem(tblBlog);
    if (blogStr != null) {
        lstBlogs = JSON.parse(blogStr);
    }

    return lstBlogs;
}

const runBlog = () => {
    readBlog();
}
runBlog();

const createBlog = (title, author, content) => {

    let lstBlog = getBlogs();

    const blog = {
        Id: uuidv4(),
        Title: title,
        Author: author,
        Content: content
    }

    lstBlog.push(blog);

    setLocalStorage(lstBlog);
    readBlog();
}
const editBlog = (id) => {
    let lstBlog = getBlogs();

    let lst = lstBlog.filter(x => x.Id === id);
    if (lst.length === 0) {
        console.log('No data found.');
        return;
    }

    let item = lst[0];
    $('#Title').val(item.Title);
    $('#Author').val(item.Author);
    $('#Content').val(item.Content);

    _blogId = item.Id;
}
const updateBlog = (id, title, author, content) => {
    let lstBlog = getBlogs();

    let lst = lstBlog.filter(x => x.Id === id); // array
    if (lst.length === 0) {
        console.log('No data found.');
        return;
    }

    let index = lstBlog.findIndex(x => x.Id === id);
    lstBlog[index] = {
        Id: id,
        Title: title,
        Author: author,
        Content: content
    }
    setLocalStorage(lstBlog);
    readBlog();
}

const deleteBlog = (id) => {
    Notiflix.Confirm.show(
        'Confirm',
        'Are you sure you want to delete?',
        'Yes',
        'No',
        () => {
            let lstBlog = getBlogs();
            let lst = lstBlog.filter(x => x.Id === id);
            if (lst.length === 0) {
                console.log('No data found.');
                return;
            }

            lstBlog = lstBlog.filter(x => x.Id !== id);

            setLocalStorage(lstBlog);
            successMessage("Delete Success")
            readBlog();
        },
        () => { },
        {
        },
    );

}

const uuidv4 = () => {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}
const setLocalStorage = (blogs) => {
    let jsonStr = JSON.stringify(blogs);
    localStorage.setItem(tblBlog, jsonStr);
}

$('#btnSave').click(() => {
    const title = $('#Title').val();
    const author = $('#Author').val();
    const content = $('#Content').val();

    if (_blogId === '') {
        Notiflix.Loading.circle();
        setTimeout(() => {
            createBlog(title, author, content);
            Notiflix.Loading.remove();
            successMessage("Save Success")
        }, 2000)
    }
    else {
        Notiflix.Loading.circle();
        setTimeout(() => {
            updateBlog(_blogId, title, author, content);
            Notiflix.Loading.remove();
            successMessage("Update Success");
            _blogId = '';
        }, 2000);
    }

    $('#Title').val('');
    $('#Author').val('');
    $('#Content').val('');

    $('#Title').focus();
})
const successMessage = (message) => {
    Notiflix.Notify.success(message);
}