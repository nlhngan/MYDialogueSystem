load_backend: loaded RPC backend from llama-b7445-bin-win-cpu-x64\ggml-rpc.dll
load_backend: loaded CPU backend from llama-b7445-bin-win-cpu-x64\ggml-cpu-haswell.dll
main: n_parallel is set to auto, using n_parallel = 4 and kv_unified = true
build: 7445 (4b2a4778f) with Clang 19.1.5 for Windows x86_64
system info: n_threads = 6, n_threads_batch = 6, total_threads = 12

system_info: n_threads = 6 (n_threads_batch = 6) / 12 | CPU : SSE3 = 1 | SSSE3 = 1 | AVX = 1 | AVX2 = 1 | F16C = 1 | FMA = 1 | BMI2 = 1 | LLAMAFILE = 1 | OPENMP = 1 | REPACK = 1 |

init: using 11 threads for HTTP server
start: binding port with default address family
main: loading model
srv    load_model: loading model 'gemma-2-2b-it-Q4_K_M.gguf'
common_init_result: fitting params to device memory, to report bugs during this step use -fit off (or --verbose if you can't)
llama_params_fit_impl: no devices with dedicated memory found
llama_params_fit: successfully fit params to free device memory
llama_params_fit: fitting params to free memory took 0.86 seconds
llama_model_loader: loaded meta data with 39 key-value pairs and 288 tensors from gemma-2-2b-it-Q4_K_M.gguf (version GGUF V3 (latest))
llama_model_loader: Dumping metadata keys/values. Note: KV overrides do not apply in this output.
llama_model_loader: - kv   0:                       general.architecture str              = gemma2
llama_model_loader: - kv   1:                               general.type str              = model
llama_model_loader: - kv   2:                               general.name str              = Gemma 2 2b It
llama_model_loader: - kv   3:                           general.finetune str              = it
llama_model_loader: - kv   4:                           general.basename str              = gemma-2
llama_model_loader: - kv   5:                         general.size_label str              = 2B
llama_model_loader: - kv   6:                            general.license str              = gemma
llama_model_loader: - kv   7:                               general.tags arr[str,2]       = ["conversational", "text-generation"]
llama_model_loader: - kv   8:                      gemma2.context_length u32              = 8192
llama_model_loader: - kv   9:                    gemma2.embedding_length u32              = 2304
llama_model_loader: - kv  10:                         gemma2.block_count u32              = 26
llama_model_loader: - kv  11:                 gemma2.feed_forward_length u32              = 9216
llama_model_loader: - kv  12:                gemma2.attention.head_count u32              = 8
llama_model_loader: - kv  13:             gemma2.attention.head_count_kv u32              = 4
llama_model_loader: - kv  14:    gemma2.attention.layer_norm_rms_epsilon f32              = 0.000001
llama_model_loader: - kv  15:                gemma2.attention.key_length u32              = 256
llama_model_loader: - kv  16:              gemma2.attention.value_length u32              = 256
llama_model_loader: - kv  17:                          general.file_type u32              = 15
llama_model_loader: - kv  18:              gemma2.attn_logit_softcapping f32              = 50.000000
llama_model_loader: - kv  19:             gemma2.final_logit_softcapping f32              = 30.000000
llama_model_loader: - kv  20:            gemma2.attention.sliding_window u32              = 4096
llama_model_loader: - kv  21:                       tokenizer.ggml.model str              = llama
llama_model_loader: - kv  22:                         tokenizer.ggml.pre str              = default
llama_model_loader: - kv  23:                      tokenizer.ggml.tokens arr[str,256000]  = ["<pad>", "<eos>", "<bos>", "<unk>", ...
llama_model_loader: - kv  24:                      tokenizer.ggml.scores arr[f32,256000]  = [-1000.000000, -1000.000000, -1000.00...
llama_model_loader: - kv  25:                  tokenizer.ggml.token_type arr[i32,256000]  = [3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, ...
llama_model_loader: - kv  26:                tokenizer.ggml.bos_token_id u32              = 2
llama_model_loader: - kv  27:                tokenizer.ggml.eos_token_id u32              = 1
llama_model_loader: - kv  28:            tokenizer.ggml.unknown_token_id u32              = 3
llama_model_loader: - kv  29:            tokenizer.ggml.padding_token_id u32              = 0
llama_model_loader: - kv  30:               tokenizer.ggml.add_bos_token bool             = true
llama_model_loader: - kv  31:               tokenizer.ggml.add_eos_token bool             = false
llama_model_loader: - kv  32:                    tokenizer.chat_template str              = {{ bos_token }}{% if messages[0]['rol...
llama_model_loader: - kv  33:            tokenizer.ggml.add_space_prefix bool             = false
llama_model_loader: - kv  34:               general.quantization_version u32              = 2
llama_model_loader: - kv  35:                      quantize.imatrix.file str              = /models_out/gemma-2-2b-it-GGUF/gemma-...
llama_model_loader: - kv  36:                   quantize.imatrix.dataset str              = /training_dir/calibration_datav3.txt
llama_model_loader: - kv  37:             quantize.imatrix.entries_count i32              = 182
llama_model_loader: - kv  38:              quantize.imatrix.chunks_count i32              = 128
llama_model_loader: - type  f32:  105 tensors
llama_model_loader: - type q4_K:  156 tensors
llama_model_loader: - type q6_K:   27 tensors
print_info: file format = GGUF V3 (latest)
print_info: file type   = Q4_K - Medium
print_info: file size   = 1.59 GiB (5.21 BPW)
load: special_eos_id is not in special_eog_ids - the tokenizer config may be incorrect
load: printing all EOG tokens:
load:   - 1 ('<eos>')
load:   - 107 ('<end_of_turn>')
load: special tokens cache size = 249
load: token to piece cache size = 1.6014 MB
print_info: arch             = gemma2
print_info: vocab_only       = 0
print_info: no_alloc         = 0
print_info: n_ctx_train      = 8192
print_info: n_embd           = 2304
print_info: n_embd_inp       = 2304
print_info: n_layer          = 26
print_info: n_head           = 8
print_info: n_head_kv        = 4
print_info: n_rot            = 256
print_info: n_swa            = 4096
print_info: is_swa_any       = 1
print_info: n_embd_head_k    = 256
print_info: n_embd_head_v    = 256
print_info: n_gqa            = 2
print_info: n_embd_k_gqa     = 1024
print_info: n_embd_v_gqa     = 1024
print_info: f_norm_eps       = 0.0e+00
print_info: f_norm_rms_eps   = 1.0e-06
print_info: f_clamp_kqv      = 0.0e+00
print_info: f_max_alibi_bias = 0.0e+00
print_info: f_logit_scale    = 0.0e+00
print_info: f_attn_scale     = 6.2e-02
print_info: n_ff             = 9216
print_info: n_expert         = 0
print_info: n_expert_used    = 0
print_info: n_expert_groups  = 0
print_info: n_group_used     = 0
print_info: causal attn      = 1
print_info: pooling type     = 0
print_info: rope type        = 2
print_info: rope scaling     = linear
print_info: freq_base_train  = 10000.0
print_info: freq_scale_train = 1
print_info: n_ctx_orig_yarn  = 8192
print_info: rope_yarn_log_mul= 0.0000
print_info: rope_finetuned   = unknown
print_info: model type       = 2B
print_info: model params     = 2.61 B
print_info: general.name     = Gemma 2 2b It
print_info: vocab type       = SPM
print_info: n_vocab          = 256000
print_info: n_merges         = 0
print_info: BOS token        = 2 '<bos>'
print_info: EOS token        = 1 '<eos>'
print_info: EOT token        = 107 '<end_of_turn>'
print_info: UNK token        = 3 '<unk>'
print_info: PAD token        = 0 '<pad>'
print_info: LF token         = 227 '<0x0A>'
print_info: EOG token        = 1 '<eos>'
print_info: EOG token        = 107 '<end_of_turn>'
print_info: max token length = 48
load_tensors: loading model tensors, this can take a while... (mmap = true)
load_tensors: offloading 26 repeating layers to GPU
load_tensors: offloading output layer to GPU
load_tensors: offloaded 27/27 layers to GPU
load_tensors:   CPU_Mapped model buffer size =  1623.67 MiB
load_tensors:   CPU_REPACK model buffer size =   921.38 MiB
.........................................................................
common_init_result: added <eos> logit bias = -inf
common_init_result: added <end_of_turn> logit bias = -inf
llama_context: constructing llama_context
llama_context: n_seq_max     = 4
llama_context: n_ctx         = 8192
llama_context: n_ctx_seq     = 8192
llama_context: n_batch       = 2048
llama_context: n_ubatch      = 512
llama_context: causal_attn   = 1
llama_context: flash_attn    = auto
llama_context: kv_unified    = true
llama_context: freq_base     = 10000.0
llama_context: freq_scale    = 1
llama_context:        CPU  output buffer size =     3.91 MiB
llama_kv_cache_iswa: creating non-SWA KV cache, size = 8192 cells
llama_kv_cache:        CPU KV buffer size =   416.00 MiB
llama_kv_cache: size =  416.00 MiB (  8192 cells,  13 layers,  4/1 seqs), K (f16):  208.00 MiB, V (f16):  208.00 MiB
llama_kv_cache_iswa: creating     SWA KV cache, size = 8192 cells
llama_kv_cache:        CPU KV buffer size =   416.00 MiB
llama_kv_cache: size =  416.00 MiB (  8192 cells,  13 layers,  4/1 seqs), K (f16):  208.00 MiB, V (f16):  208.00 MiB
llama_context: Flash Attention was auto, set to enabled
llama_context:        CPU compute buffer size =   509.00 MiB
llama_context: graph nodes  = 948
llama_context: graph splits = 1
common_init_from_params: warming up the model with an empty run - please wait ... (--no-warmup to disable)
srv          init: initializing slots, n_slots = 4
slot         init: id  0 | task -1 | new slot, n_ctx = 8192
slot         init: id  1 | task -1 | new slot, n_ctx = 8192
slot         init: id  2 | task -1 | new slot, n_ctx = 8192
slot         init: id  3 | task -1 | new slot, n_ctx = 8192
srv          init: prompt cache is enabled, size limit: 8192 MiB
srv          init: use `--cache-ram 0` to disable the prompt cache
srv          init: for more info see https://github.com/ggml-org/llama.cpp/pull/16391
srv          init: thinking = 0
init: chat template, chat_template: {{ bos_token }}{% if messages[0]['role'] == 'system' %}{{ raise_exception('System role not supported') }}{% endif %}{% for message in messages %}{% if (message['role'] == 'user') != (loop.index0 % 2 == 0) %}{{ raise_exception('Conversation roles must alternate user/assistant/user/assistant/...') }}{% endif %}{% if (message['role'] == 'assistant') %}{% set role = 'model' %}{% else %}{% set role = message['role'] %}{% endif %}{{ '<start_of_turn>' + role + '
' + message['content'] | trim + '<end_of_turn>
' }}{% endfor %}{% if add_generation_prompt %}{{'<start_of_turn>model
'}}{% endif %}, example_format: '<start_of_turn>user
You are a helpful assistant
Hello<end_of_turn>
<start_of_turn>model
Hi there<end_of_turn>
<start_of_turn>user
How are you?<end_of_turn>
<start_of_turn>model
'
main: model loaded
main: server is listening on http://127.0.0.1:8080
main: starting the main loop...
srv  update_slots: all slots are idle
srv  params_from_: Chat format: Content-only
slot get_availabl: id  3 | task -1 | selected slot by LRU, t_last = -1
slot launch_slot_: id  3 | task -1 | sampler chain: logits -> penalties -> dry -> top-n-sigma -> top-k -> typical -> top-p -> min-p -> xtc -> temp-ext -> dist
slot launch_slot_: id  3 | task 0 | processing task
slot update_slots: id  3 | task 0 | new prompt, n_ctx_slot = 8192, n_keep = 0, task.n_tokens = 52
slot update_slots: id  3 | task 0 | n_tokens = 0, memory_seq_rm [0, end)
slot update_slots: id  3 | task 0 | prompt processing progress, n_tokens = 52, batch.n_tokens = 52, progress = 1.000000
slot update_slots: id  3 | task 0 | prompt done, n_tokens = 52, batch.n_tokens = 52
slot print_timing: id  3 | task 0 |
prompt eval time =     877.88 ms /    52 tokens (   16.88 ms per token,    59.23 tokens per second)
       eval time =     475.84 ms /     7 tokens (   67.98 ms per token,    14.71 tokens per second)
      total time =    1353.71 ms /    59 tokens
slot      release: id  3 | task 0 | stop processing: n_tokens = 58, truncated = 0
srv  update_slots: all slots are idle
srv  log_server_r: request: POST /v1/chat/completions 127.0.0.1 200
srv  params_from_: Chat format: Content-only
slot get_availabl: id  3 | task -1 | selected slot by LCP similarity, sim_best = 0.630 (> 0.100 thold), f_keep = 0.793
slot launch_slot_: id  3 | task -1 | sampler chain: logits -> penalties -> dry -> top-n-sigma -> top-k -> typical -> top-p -> min-p -> xtc -> temp-ext -> dist
slot launch_slot_: id  3 | task 8 | processing task
slot update_slots: id  3 | task 8 | new prompt, n_ctx_slot = 8192, n_keep = 0, task.n_tokens = 73
slot update_slots: id  3 | task 8 | n_tokens = 46, memory_seq_rm [46, end)
slot update_slots: id  3 | task 8 | prompt processing progress, n_tokens = 73, batch.n_tokens = 27, progress = 1.000000
slot update_slots: id  3 | task 8 | prompt done, n_tokens = 73, batch.n_tokens = 27
slot print_timing: id  3 | task 8 |
prompt eval time =     503.67 ms /    27 tokens (   18.65 ms per token,    53.61 tokens per second)
       eval time =    1048.08 ms /    15 tokens (   69.87 ms per token,    14.31 tokens per second)
      total time =    1551.75 ms /    42 tokens
slot      release: id  3 | task 8 | stop processing: n_tokens = 87, truncated = 0
srv  update_slots: all slots are idle
srv  log_server_r: request: POST /v1/chat/completions 127.0.0.1 200
srv  params_from_: Chat format: Content-only
slot get_availabl: id  3 | task -1 | selected slot by LCP similarity, sim_best = 0.697 (> 0.100 thold), f_keep = 0.529
slot launch_slot_: id  3 | task -1 | sampler chain: logits -> penalties -> dry -> top-n-sigma -> top-k -> typical -> top-p -> min-p -> xtc -> temp-ext -> dist
slot launch_slot_: id  3 | task 24 | processing task
slot update_slots: id  3 | task 24 | new prompt, n_ctx_slot = 8192, n_keep = 0, task.n_tokens = 66
slot update_slots: id  3 | task 24 | n_tokens = 46, memory_seq_rm [46, end)
slot update_slots: id  3 | task 24 | prompt processing progress, n_tokens = 66, batch.n_tokens = 20, progress = 1.000000
slot update_slots: id  3 | task 24 | prompt done, n_tokens = 66, batch.n_tokens = 20
slot print_timing: id  3 | task 24 |
prompt eval time =     374.07 ms /    20 tokens (   18.70 ms per token,    53.47 tokens per second)
       eval time =    2245.95 ms /    31 tokens (   72.45 ms per token,    13.80 tokens per second)
      total time =    2620.02 ms /    51 tokens
slot      release: id  3 | task 24 | stop processing: n_tokens = 96, truncated = 0
srv  update_slots: all slots are idle
srv  log_server_r: request: POST /v1/chat/completions 127.0.0.1 200
srv    operator(): operator(): cleaning up before exit...
llama_memory_breakdown_print: | memory breakdown [MiB] | total   free    self   model   context   compute    unaccounted |
llama_memory_breakdown_print: |   - Host               |                 2964 =  1623 +     832 +     509                |
llama_memory_breakdown_print: |   - CPU_REPACK         |                  921 =   921 +       0 +       0                |