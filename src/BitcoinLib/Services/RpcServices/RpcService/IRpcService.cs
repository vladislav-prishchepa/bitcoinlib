// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.Requests.AddNode;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Requests.SignRawTransaction;
using BitcoinLib.Responses;

namespace BitcoinLib.Services.RpcServices.RpcService
{
    public interface IRpcService
    {
        #region Blockchain

        Task<string> GetBestBlockHashAsync(CancellationToken cancellationToken = default);
        Task<GetBlockResponse> GetBlockAsync(string hash, bool verbose = true, CancellationToken cancellationToken = default);
        Task<GetBlockchainInfoResponse> GetBlockchainInfoAsync(CancellationToken cancellationToken = default);
        Task<uint> GetBlockCountAsync(CancellationToken cancellationToken = default);
        Task<string> GetBlockHashAsync(long index, CancellationToken cancellationToken = default);
        //  getblockheader
        //  getchaintips
        Task<double> GetDifficultyAsync(CancellationToken cancellationToken = default);
        Task<List<GetChainTipsResponse>> GetChainTipsAsync(CancellationToken cancellationToken = default);
        Task<GetMemPoolInfoResponse> GetMemPoolInfoAsync(CancellationToken cancellationToken = default);
        Task<GetRawMemPoolResponse> GetRawMemPoolAsync(bool verbose = false, CancellationToken cancellationToken = default);
        Task<GetTransactionResponse> GetTxOutAsync(string txId, int n, bool includeMemPool = true, CancellationToken cancellationToken = default);
        //  gettxoutproof["txid",...] ( blockhash )
        Task<GetTxOutSetInfoResponse> GetTxOutSetInfoAsync(CancellationToken cancellationToken = default);
        Task<bool> VerifyChainAsync(ushort checkLevel = 3, uint numBlocks = 288, CancellationToken cancellationToken = default); //  Note: numBlocks: 0 => ALL

        #endregion

        #region Control

        Task<GetInfoResponse> GetInfoAsync(CancellationToken cancellationToken = default);
        Task<string> HelpAsync(string command = null, CancellationToken cancellationToken = default);
        Task<string> StopAsync(CancellationToken cancellationToken = default);

        #endregion

        #region Generating

        //  generate numblocks
        Task<bool> GetGenerateAsync(CancellationToken cancellationToken = default);
        Task<string> SetGenerateAsync(bool generate, short generatingProcessorsLimit, CancellationToken cancellationToken = default);

        #endregion

        #region Mining

        Task<GetBlockTemplateResponse> GetBlockTemplateAsync(params object[] parameters);
        Task<GetBlockTemplateResponse> GetBlockTemplateAsync(CancellationToken cancellationToken, params object[] parameters);
        Task<GetMiningInfoResponse> GetMiningInfoAsync(CancellationToken cancellationToken = default);
        Task<ulong> GetNetworkHashPsAsync(uint blocks = 120, long height = -1, CancellationToken cancellationToken = default);
        Task<bool> PrioritiseTransactionAsync(string txId, decimal priorityDelta, decimal feeDelta, CancellationToken cancellationToken = default);
        Task<string> SubmitBlockAsync(string hexData, params object[] parameters);
        Task<string> SubmitBlockAsync(string hexData, CancellationToken cancellationToken, params object[] parameters);

        #endregion

        #region Network

        Task AddNodeAsync(string node, NodeAction action, CancellationToken cancellationToken = default);
        //  clearbanned
        //  disconnectnode
        Task<GetAddedNodeInfoResponse> GetAddedNodeInfoAsync(string dns, string node = null, CancellationToken cancellationToken = default);
        Task<int> GetConnectionCountAsync(CancellationToken cancellationToken = default);
        Task<GetNetTotalsResponse> GetNetTotalsAsync(CancellationToken cancellationToken = default);
        Task<GetNetworkInfoResponse> GetNetworkInfoAsync(CancellationToken cancellationToken = default);
        Task<List<GetPeerInfoResponse>> GetPeerInfoAsync(CancellationToken cancellationToken = default);
        //  listbanned
        Task PingAsync(CancellationToken cancellationToken = default);
        //  setban

        #endregion

        #region Rawtransactions

        Task<string> CreateRawTransactionAsync(CreateRawTransactionRequest rawTransaction, CancellationToken cancellationToken = default);
        Task<DecodeRawTransactionResponse> DecodeRawTransactionAsync(string rawTransactionHexString, CancellationToken cancellationToken = default);
        Task<DecodeScriptResponse> DecodeScriptAsync(string hexString, CancellationToken cancellationToken = default);
        //  fundrawtransaction
        Task<GetRawTransactionResponse> GetRawTransactionAsync(string txId, int verbose = 0, CancellationToken cancellationToken = default);
        Task<string> SendRawTransactionAsync(string rawTransactionHexString, bool? allowHighFees = false, CancellationToken cancellationToken = default);
        Task<SignRawTransactionResponse> SignRawTransactionAsync(SignRawTransactionRequest signRawTransactionRequest, CancellationToken cancellationToken = default);
        Task<SignRawTransactionWithKeyResponse> SignRawTransactionWithKeyAsync(SignRawTransactionWithKeyRequest signRawTransactionWithKeyRequest, CancellationToken cancellationToken = default);
        Task<SignRawTransactionWithWalletResponse> SignRawTransactionWithWalletAsync(SignRawTransactionWithWalletRequest signRawTransactionWithWalletRequest, CancellationToken cancellationToken = default);
        Task<GetFundRawTransactionResponse> GetFundRawTransactionAsync(string rawTransactionHex, CancellationToken cancellationToken = default);

        #endregion

        #region Util

        Task<CreateMultiSigResponse> CreateMultiSigAsync(int nRquired, List<string> publicKeys, CancellationToken cancellationToken = default);
        Task<decimal> EstimateFeeAsync(ushort nBlocks, CancellationToken cancellationToken = default);
        Task<EstimateSmartFeeResponse> EstimateSmartFeeAsync(ushort nBlocks, CancellationToken cancellationToken = default);
        Task<decimal> EstimatePriorityAsync(ushort nBlocks, CancellationToken cancellationToken = default);
        //  estimatesmartfee
        //  estimatesmartpriority
        Task<ValidateAddressResponse> ValidateAddressAsync(string bitcoinAddress, CancellationToken cancellationToken = default);
        Task<bool> VerifyMessageAsync(string bitcoinAddress, string signature, string message, CancellationToken cancellationToken = default);

        #endregion

        #region Wallet

        //  abandontransaction
        Task<string> AddMultiSigAddressAsync(int nRquired, List<string> publicKeys, string account = null, CancellationToken cancellationToken = default);
        Task<string> AddWitnessAddressAsync(string address, CancellationToken cancellationToken = default);
        Task BackupWalletAsync(string destination, CancellationToken cancellationToken = default);
        Task<string> DumpPrivKeyAsync(string bitcoinAddress, CancellationToken cancellationToken = default);
        Task DumpWalletAsync(string filename, CancellationToken cancellationToken = default);
        Task<string> GetAccountAsync(string bitcoinAddress, CancellationToken cancellationToken = default);
        Task<string> GetAccountAddressAsync(string account, CancellationToken cancellationToken = default);
        Task<List<string>> GetAddressesByAccountAsync(string account, CancellationToken cancellationToken = default);
        Task<Dictionary<string, GetAddressesByLabelResponse>> GetAddressesByLabelAsync(string label, CancellationToken cancellationToken = default);
        Task<GetAddressInfoResponse> GetAddressInfoAsync(string bitcoinAddress, CancellationToken cancellationToken = default);
        Task<decimal> GetBalanceAsync(string account = null, int minConf = 1, bool? includeWatchonly = null, CancellationToken cancellationToken = default);
        Task<string> GetNewAddressAsync(string account = "", CancellationToken cancellationToken = default);
        Task<string> GetRawChangeAddressAsync(CancellationToken cancellationToken = default);
        Task<decimal> GetReceivedByAccountAsync(string account, int minConf = 1, CancellationToken cancellationToken = default);
        Task<decimal> GetReceivedByAddressAsync(string bitcoinAddress, int minConf = 1, CancellationToken cancellationToken = default);
        Task<decimal> GetReceivedByLabelAsync(string account, int minConf = 1, CancellationToken cancellationToken = default);
        Task<GetTransactionResponse> GetTransactionAsync(string txId, bool? includeWatchonly = null, CancellationToken cancellationToken = default);
        Task<decimal> GetUnconfirmedBalanceAsync(CancellationToken cancellationToken = default);
        Task<GetWalletInfoResponse> GetWalletInfoAsync(CancellationToken cancellationToken = default);
        Task ImportAddressAsync(string address, string label = null, bool rescan = true, CancellationToken cancellationToken = default);
        Task<string> ImportPrivKeyAsync(string privateKey, string label = null, bool rescan = true, CancellationToken cancellationToken = default);
        //  importpubkey
        Task ImportWalletAsync(string filename, CancellationToken cancellationToken = default);
        Task<string> KeyPoolRefillAsync(uint newSize = 100, CancellationToken cancellationToken = default);
        Task<Dictionary<string, decimal>> ListAccountsAsync(int minConf = 1, bool? includeWatchonly = null, CancellationToken cancellationToken = default);
        Task<List<List<ListAddressGroupingsResponse>>> ListAddressGroupingsAsync(CancellationToken cancellationToken = default);
        Task<List<string>> ListLabelsAsync(CancellationToken cancellationToken = default);
        Task<string> ListLockUnspentAsync(CancellationToken cancellationToken = default);
        Task<List<ListReceivedByAccountResponse>> ListReceivedByAccountAsync(int minConf = 1, bool includeEmpty = false, bool? includeWatchonly = null, CancellationToken cancellationToken = default);
        Task<List<ListReceivedByAddressResponse>> ListReceivedByAddressAsync(int minConf = 1, bool includeEmpty = false, bool? includeWatchonly = false, CancellationToken cancellationToken = default);
        Task<List<ListReceivedByLabelResponse>> ListReceivedByLabelAsync(int minConf = 1, bool includeEmpty = false, bool? includeWatchonly = null, CancellationToken cancellationToken = default);
        Task<ListSinceBlockResponse> ListSinceBlockAsync(string blockHash = null, int targetConfirmations = 1, bool? includeWatchonly = null, CancellationToken cancellationToken = default);
        Task<List<ListTransactionsResponse>> ListTransactionsAsync(string account = null, int count = 10, int from = 0, bool? includeWatchonly = null, CancellationToken cancellationToken = default);
        Task<List<ListUnspentResponse>> ListUnspentAsync(int minConf = 1, int maxConf = 9999999, List<string> addresses = null, CancellationToken cancellationToken = default);
        Task<bool> LockUnspentAsync(bool unlock, IList<ListUnspentResponse> listUnspentResponses, CancellationToken cancellationToken = default);
        Task<bool> MoveAsync(string fromAccount, string toAccount, decimal amount, int minConf = 1, string comment = "", CancellationToken cancellationToken = default);
        Task<string> SendFromAsync(string fromAccount, string toBitcoinAddress, decimal amount, int minConf = 1, string comment = null, string commentTo = null, CancellationToken cancellationToken = default);
        Task<string> SendManyAsync(string fromAccount, Dictionary<string, decimal> toBitcoinAddress, int minConf = 1, string comment = null, CancellationToken cancellationToken = default);
        Task<string> SendToAddressAsync(string bitcoinAddress, decimal amount, string comment = null, string commentTo = null, bool subtractFeeFromAmount = false, CancellationToken cancellationToken = default);
        Task<string> SetAccountAsync(string bitcoinAddress, string account, CancellationToken cancellationToken = default);
        Task<string> SetLabelAsync(string bitcoinAddress, string label, CancellationToken cancellationToken = default);
        Task<string> SetTxFeeAsync(decimal amount, CancellationToken cancellationToken = default);
        Task<string> SignMessageAsync(string bitcoinAddress, string message, CancellationToken cancellationToken = default);
        Task<string> WalletLockAsync(CancellationToken cancellationToken = default);
        Task<string> WalletPassphraseAsync(string passphrase, int timeoutInSeconds, CancellationToken cancellationToken = default);
        Task<string> WalletPassphraseChangeAsync(string oldPassphrase, string newPassphrase, CancellationToken cancellationToken = default);

        #endregion
    }
}