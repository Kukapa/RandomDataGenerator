
const maxInt = 2147483647;

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('generate_csv').addEventListener('click', () => {
        let csv_data = [];

        const rows = document.getElementsByTagName('tr');
        for (let i = 0; i < rows.length; i++) {

            const cols = rows[i].querySelectorAll('td,th');

            const csvrow = [];
            for (let j = 0; j < cols.length; j++) {
                let cellData = cols[j].innerText;

                if (cellData.includes(',') || cellData.includes('"')) {
                    cellData = `"${cellData.replace(/"/g, '""')}"`;
                }

                csvrow.push(cellData);
            }

            csv_data.push(csvrow.join(","));
        }

        csv_data = csv_data.join('\n');

        downloadCSVFile(csv_data);
    });

    function downloadCSVFile(csv_data) {
        const temp_link = document.createElement('a');

        temp_link.download = "tyfordownload.csv";
        temp_link.href = 'data:text/csv;charset=utf-8,%EF%BB%BF' + encodeURI(csv_data);

        temp_link.style.display = "none";
        document.body.appendChild(temp_link);

        temp_link.click();
        document.body.removeChild(temp_link);
    }

    const rangeInput = document.getElementById('err-range');
    const numberInput = document.getElementById('err-input');
    const locSelect = document.getElementById('loc-select');
    const seedInput = document.getElementById('seed');

    document.getElementById('generate_seed').addEventListener('click', () => {
        seedInput.value = getRandomInt32();
        seedInput.dispatchEvent(new Event('change', { cancelable: false }));
    });

    rangeInput.addEventListener('input', () => {
        numberInput.value = rangeInput.value;
    });

    numberInput.addEventListener('input', () => {
        rangeInput.value = numberInput.value;
    });

    rangeInput.addEventListener('change', obtainNewData);
    numberInput.addEventListener('change', obtainNewData);
    locSelect.addEventListener('change', obtainNewData);
    seedInput.addEventListener('change', obtainNewData);

    let currentPage = 1;
    const pageSize = 20;
    let isLoading = false;
    let allDataLoaded = false;

    const container = document.getElementById('main-table');

    loadMoreData();

    window.addEventListener('scroll', () => {
        if (window.scrollY + window.innerHeight >= container.offsetTop + container.offsetHeight) {
            if (!isLoading && !allDataLoaded) {
                loadMoreData();
            }
        }
    });

    function loadMoreData() {
        isLoading = true;
        const seed = seedInput.value;
        const errorProb = numberInput.value;
        const locale = locSelect.value;

        fetch(`/api/persons?page=${currentPage}&size=${pageSize}&seed=${seed}&errorProb=${errorProb}&locale=${locale}`)
            .then(response => response.json())
            .then(data => {
                if (data.length === 0) {
                    allDataLoaded = true;
                } else {
                    appendRows(data);
                    currentPage++;
                }
            })
            .catch(error => {
                console.error('Error fetching data:', error);
            })
            .finally(() => {
                isLoading = false;
            });
    }

    function obtainNewData() {
        currentPage = 1;
        refreshTable();
        loadMoreData();
    }

    function appendRows(data) {
        const tableBody = container.querySelector('tbody');
        data.forEach(row => {
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
            <td>${row.index + 1}</td>
            <td>${row.identifier}</td>
            <td>${row.fullName}</td>
            <td>${row.fullAddress}</td>
            <td>${row.phoneNumber}</td>
        `;
            tableBody.appendChild(newRow);
        });
    }

    function refreshTable() {
        const tableBody = container.querySelector('tbody');
        const trList = tableBody.querySelectorAll('tr');
        trList.forEach((element) => {
            tableBody.removeChild(element);
        });
    }

    function getRandomInt32() {
        return Math.floor(Math.random() * (maxInt + 1));
    }
});